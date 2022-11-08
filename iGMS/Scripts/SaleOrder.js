//Chọn cửa hàng hay kho hàng để xuất

$('input[type="radio"]').click(function () {
    var radio = document.getElementsByName('radio');
    for (let i = 0; i < radio.length; i++) {
        if (radio[i].checked) {
            switch (radio[i].value) {
                case "CH": $('#warehouse').attr("disabled", true)
                    $('#store').attr("disabled", false);
                    $('#warehouse').val(-1)
                    break;
                case "KH": $('#store').attr("disabled", true);
                    $('#warehouse').attr("disabled", false);
                    $('#store').val(-1)
                    break;
            }
        }
    }
})

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Tìm Hàng hóa trong kho

$('#seachidgood').keypress(function (event) {
    var warehouse = $('#warehouse option:selected').val();
    var store = $('#store option:selected').val();
    if (warehouse == -1 && store == -1) {
        alert("Chọn Nơi Xuất")
    } else {
        if (event.which == 13) {
            var id = $('#seachidgood').val().trim();
            var amount = $('#amount' + id + '').val() == null ? 0 : Number($('#amount' + id + '').val()) + 1;
            Good(id);
            validateAmount(id, amount)        
        }
    }
})

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Hiển thị hàng hóa để bán

function Good(id) {
    var warehouse = $('#warehouse option:selected').val();
    var store = $('#store option:selected').val();
    var H = "";
    if (warehouse == -1) {
        H = store
    } else {
        H = warehouse
    }
    $.ajax({
        url: '/saleorder/Good',
        type: 'get',
        data: {
            id,H
        },
        success: function (data) {
            if (data.code == 200) {
                var Stt = 1;
                $.each(data.c, function (k, v) {
                    var ids = $('.IDGOOD').map(function () {
                        return this.id;
                    }).get();
                    if (ids.includes(id)) {
                        var amounts = $('#' + id + ' #amount' + id + '').val();
                        var price = $('#' + id + ' #price' + id + '').val();
                        var discount = $('#' + id + ' #discount' + id + '').val();
                        $('#' + id + ' #amount' + v.id + '').val('');
                        $('#' + id + ' #amount' + v.id + '').val(1);
                        $('#' + id + ' #totalmoney' + id + '').empty()
                        var sum = Number(price) * (Number(amounts) + 1);
                        $('#' + id + ' #sumpricegoods' + id + '').empty();
                        $('#' + id + ' #sumpricegoods' + id + '').append(sum + (sum * (Number(discount) / 100)))
                    } else {
                        let table = '<tr id="' + v.idgood + '" role="row" class="odd IDGOOD">';
                        table += '<td>' + (Stt++) + '</td>'
                        table += '<td class="' + v.idgood + '">' + v.idgood + '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td><input type="number" value="0" id="amount' + v.idgood + '" name="amount" /></td>'
                        table += '<td>'
                        table += '<input name="tags-outside" disabled  placeholder="Nhập Đủ Mã Serial" class="tagify--outside form-control" id="epc' + v.idgood + '" />'
                        table += '</td>'
                        table += '<td><input type="text" value="' + v.price + '" id="price' + v.idgood + '" name="price" /></td>'
                        table += '<td class="sumpricegoods" id="sumpricegoods' + v.idgood + '"></td>'
                        table += '<td name="delete"><img src="/Images/icons8-remove-38.png" /></td>'
                        table += '</tr>';
                        $('#tbd').append(table);
                        var amount = $('#amount' + v.idgood + '').val();
                        $('#sumpricegoods' + v.idgood + '').append(v.price * amount + (v.price * v.discount / 100))
                    }
                })

                Tien(id)

                //Nhấp số lượng cho hàng hóa

                $('input[name="amount"]').keypress(function (event) {
                    if (event.which == 13) {
                        var id = $(this).closest('tr').attr('id');
                        var amount = $('#amount' + id + '').val()
                        Tien(id)
                        validateAmount(id, amount)
                        EPC(id,amount,H)
                    }
                })

                //Nhập Giá cho hàng hóa

                $('input[name="price"]').keypress(function (event) {
                    if (event.which == 13) {
                        var id = $(this).closest('tr').attr('id')
                        Tien(id)
                    }
                })
                Tong()

                //Xóa Hàng hóa

                $('td[name="delete"]').click(function () {
                    var id = $(this).closest('tr').attr('id');
                    $('#' + id + '').remove();
                })
            } else if (data.code == 1) {
                alert(data.msg)
            }
        }
    })
}

//Tổng tiền trên 1 mặt hàng

function Tien(id) {
    $('#sumpricegoods' + id + '').empty()
    var amount = $('#amount' + id + '').val();
    var price = $('#price' + id + '').val();
    var sum = amount * price;
    $('#sumpricegoods' + id + '').append(sum)
    Tong();
}

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Tổng Tiền cho đơn hàng bán

function Tong() {
    $('#sumprice').empty()
    $('#liabilities').empty()
    var ids = $('.sumpricegoods').map(function () {
        return this.innerHTML;
    }).get();
    var sum = 0;
    for (let i = 0; i < ids.length; i++) {
        sum += Number(ids[i])
    }
    $('#sumprice').append(sum)
    var partialpay = $('#partialpay').val();
    $('#liabilities').append(sum - Number(partialpay))
}

$('#partialpay').keyup(function () {
    Tong()
})

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Kiểm tra số lượng hàng còn trong kho

function validateAmount(id, amount) {
    var warehouse = $('#warehouse option:selected').val();
    var store = $('#store option:selected').val();
    var H = "";
    if (warehouse == -1) {
        H = store
    } else {
        H = warehouse
    }
    $.ajax({
        url: '/saleorder/TonKho',
        type: 'get',
        data: {
            H, id
        },
        success: function (data) {
            if (data.code == 200) {
                if (amount > data.c) {
                    alert("Hàng Không Đủ, Chỉ Còn " + data.c + "  !!!")
                    $('#amount' + id + '').val(data.c)
                    Tien(id)
                }
            }
        }
    })
}

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Kiểm tra các serial còn trong kho

function EPC(id,amount,H) {
    var input = document.getElementById('epc' + id + '');
    // init Tagify script on the above inputs
    $.ajax({
        url: '/saleorder/EPCCon',
        type: 'get',
        data: {
            id,H
        },
        success: function (data) {
            if (data.code == 200) {
                let a = ''
                $.each(data.c, function (k, v) {
                    a += v.epc + ","
                })
                const myArray = a.split(",");
                new Tagify(input, {
                    enforceWhitelist: false,
                    whitelist: myArray,
                    maxTags: Number(amount),
                    dropdown: {
                        position: "input",
                        enabled: 0// always opens dropdown when input gets focus
                    },
                    editTags: false,
                    readonly: false
                })
            }
        }
    })
}

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Quet hàng hóa

$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        Change_IdGood(barcode)
    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Đổi mã barcode thành mã hàng hóa

function Change_IdGood(barcode) {
    $.ajax({
        url: '/saleorder/Change_IdGood',
        type: 'get',
        data: {
            barcode
        },
        success: function (data) {
            if (data.code == 200) {
                Good(data.c)
            }
            else {
                alert("Thất bại")
            }
        }
    })
}

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//Thêm đơn hàng bán vào cơ sở dữ liệu

function Add() {
    var name = $('#name').val().trim();
    var warehouse = $('#warehouse option:selected').val()
    var store = $('#store option:selected').val()
    var check = $('#check').is(":checked");
    var H = "";
    var customerKH = $('#customerKH option:selected').val()
    var user = $('#user option:selected').val()
    var paymethod = $('#paymethod option:selected').val()
    var sumprice = document.getElementById('sumprice').innerText;
    var partialpay = $('#partialpay').val().trim()
    var liabilities = document.getElementById('liabilities').innerText;
    var datepay = $('#datepay').val().trim()
    var deliverydate = $('#deliverydate').val().trim()
    var des = $('#des').val().trim()
    if (name.length <= 0) {
        alert("Nhập Tên Đơn hàng !!!")
        return;
    }
    if (check == true) {
        H = warehouse
        if (warehouse == -1) {
            alert("Chọn Nơi Xuất !!!")
            return;
        }
    } else {
        H = store
        if (store == -1) {
            alert("Chọn Nơi Xuất !!!")
            return;
        }
    }
    if (customerKH == -1) {
        alert("Chọn khách hàng !!!")
        return;
    }
    if (user == -1) {
        alert("Chọn Nhân Viên !!!")
        return;
    } if (paymethod ==-1) {
        alert("Chọn Phương Thức Thanh Toán !!!")
        return;
    } if (partialpay.length <= 0) {
        alert("Nhập Tiền Đã Trá !!!")
        return;
    }
    if (datepay.length <= 0) {
        alert("Nhập Thời hạn Thanh Toán !!!")
        return;
    }
    if (deliverydate.length <= 0) {
        alert("Nhập Ngày Nhận Hàng !!!")
        return;
    }
    $.ajax({
        url: '/saleorder/Add',
        type: 'post',
        data: {
            name, H, customerKH, user, paymethod, sumprice, partialpay, liabilities, datepay, deliverydate, des
        },
        success: function (data) {
            if (data.code == 200) {
                var ids = $('.IDGOOD').map(function () {
                    return this.id;
                }).get();
                for (let i = 0; i < ids.length; i++) {
                    var id = ids[i];
                    var amount = $('#' + id + ' #amount' + id + '').val()
                    var price = $('#' + id + ' #price' + id + '').val()
                    var sumpricegoods = $('#' + id + ' #sumpricegoods' + id + '').text();
                    Xuat(id, H, amount, price, sumpricegoods)
                }
            } 
            else {
                alert("Thất bại")
            }
        },
    })
}

function Xuat(id, H, amount, price, sumpricegoods) {
    if (amount == 0) {
        alert("Chưa Nhập Số lượng  Cho " + id + "!!!")
        return;
    }
    var tags = JSON.parse($('#epc' + id + '').val())
    var TagArray = [];
    //Convert to array
    for (let j = 0; j < tags.length; j++) {
        TagArray.push(tags[j].value)
    }

    for (let k = 0; k < TagArray.length; k++) {
        var epc = TagArray[k]

        if (TagArray.length < amount) {
            alert("Chưa Nhập Đủ Mã Serial Cho " + id + " !!!")
            return;
        }
        if (epc.length < 19) {
            alert("Giá Trị " + epc + " Chưa Đủ !!!")
            return;
        }
        $.ajax({
            url: '/saleorder/AddDetails',
            type: 'post',
            data: {
                id, epc, H, amount, price, sumpricegoods
            },
            success: function (data) {
                if (data.code == 200) {
                    $('#BILL').modal('show')
                    BILL()
                }
                else {
                    alert("Thất bại")
                }
            },
        })
    }

}

//End

//----------------------------------------------------------------------------------------------------------------------------------------------

//BILL thông tin đơn hàng vừa bán

function BILL() {
    var warehouse = $('#warehouse option:selected').val()
    var store = $('#store option:selected').val()
    var check = $('#check').is(":checked");
    var H = "";
    if (check == true) {
        H = warehouse
    } else {
        H = store
    }
    $.ajax({
        url: '/saleorder/Bill',
        type: 'get',
        data: {
            H
        },
        success: function (data) {
            $('span[name="nameware"]').empty()
            $('span[name="iddh"]').empty()
            $('span[name="datedh"]').empty()
            $('span[name="customer"]').empty()
            $('span[name="address"]').empty()
            $('span[name="paydh"]').empty()
            $('span[name="datepaydh"]').empty()
            $('span[name="Tong"]').empty()
            $('span[name="sumpricedh"]').empty()
            $('#tbdmodal').empty()
            $('#qrcode').empty()
            var Stt = 1;
            if (data.code == 200) {
                $.each(data.a, function (k, v) {
                    $('span[name="nameware"]').append(v.name)
                })
                $.each(data.c, function (k, v) {
                    $('span[name="iddh"]').append("00000"+v.id)
                    $('span[name="datedh"]').append(v.createdate)
                    $('span[name="customer"]').append(v.customer)
                    $('span[name="address"]').append(v.address)
                    $('span[name="paydh"]').append(v.paymethod)
                    $('span[name="datepaydh"]').append(v.datepay)
                    $('span[name="sumpricedh"]').append(v.sumprice)
                    $('span[name="Tong"]').append(to_vietnamese(v.sumprice))
                    new QRCode(document.getElementById("qrcode"), {
                        text: '00000' + v.id,
                        width: 100,
                        height: 100,

                    })
                })
                $.each(data.e, function (k, v) {
                    let table = '<tr>';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.idgood + '</td>'
                    table += '<td>' + v.name + '</td>'  
                    table += '<td>' + v.price + '</td>'       
                    table += '<td class="modalsumprice">' + v.price + '</td></tr>'
                    $('#tbdmodal').append(table)
                })
                var modaldiscounts = $('.modaldiscount').map(function () {
                    return this.innerText;
                }).get();
                var modaltaxs = $('.modaltax').map(function () {
                    return this.innerText;
                }).get();
                var modaldiscount = 0;
                var modaltax = 0;
                for (let i = 0; i < modaldiscounts.length; i++) {
                    modaldiscount += Number(modaldiscounts[i])
                }
                for (let i = 0; i < modaltaxs.length; i++) {
                    modaltax += Number(modaltaxs[i])
                }
                $('span[name="sumpricediscount"]').append(modaldiscount)
                $('span[name="sumpricetax"]').append(modaltax)
            }
            else {
                alert("Tạo Đơn Vị Thất Bại")
            }
        }
    })

}

$('#warehouse').change(function () {
    $('#tbd').empty()
})

$('#store').change(function () {
    $('#tbd').empty()
})




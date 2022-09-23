//-------------------chọn nơi xuất hàng--------------
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
//-------------------tim hang hoa
$('#seachidgood').keypress(function(event){
    if (event.which == 13) {
        var id = $('#seachidgood').val().trim();

        Good(id);
        
    }
})

function Good(id) {
    $.ajax({
        url: '/saleorder/Good',
        type: 'get',
        data: {
            id
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
                        $('#' + id + ' #amount' + v.id + '').val(Number(amounts) + 1);
                        $('#' + id + ' #totalmoney' + id + '').empty()
                        var sum = Number(price) * (Number(amounts) + 1);
                        $('#' + id + ' #sumpricegoods' + id + '').empty();
                        $('#' + id + ' #sumpricegoods' + id + '').append(sum + (sum * (Number(discount) / 100)))
                    } else {
                        let table = '<tr id="' + v.id + '" role="row" class="odd IDGOOD">';
                        table += '<td>' + (Stt++) + '</td>'
                        table += '<td class="' + v.id + '">' + v.id + '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td>' + v.size + '</td>'
                        table += '<td><input type="number" value="1" id="amount' + v.id + '" name="amount" /></td>'
                        table += '<td><input type="text" value="' + v.price + '" id="price' + v.id + '" name="price" /></td>'
                        table += '<td class="sumpricegoods" id="sumpricegoods' + v.id + '"></td>'
                        table += '<td name="delete"><img src="/Images/icons8-remove-38.png" /></td>'
                        table += '</tr>';
                        $('#tbd').append(table);
                        var amount = $('#amount' + v.id + '').val();
                        $('#sumpricegoods' + v.id + '').append(v.price * amount + (v.price * v.discount / 100))
                    }
                    //-----------Nhap số liệu-----------
                    $('input[name="amount"]').keypress(function (event) {
                        if (event.which == 13) {
                            var id = $(this).closest('tr').attr('id');
                            var amount = $('#amount' + id + '').val()

                            Tien(id)
                            validateAmount(id, amount)
                        }
                    })
                    $('input[name="price"]').keypress(function (event) {
                        if (event.which == 13) {
                            var id = $(this).closest('tr').attr('id')
                            Tien(id)
                        }

                    })
                    Tong()
                    //-------xóa
                    $('td[name="delete"]').click(function () {
                        var id = $(this).closest('tr').attr('id');
                        $('#' + id + '').remove();
                    })
                })
            }
        }
    })
}

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
            H,id
        },
        success: function (data) {
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    if (amount > v.qty) {
                        alert("Hàng Không Đủ")
                    }
                })
            }
        }
    })
}

function Tien(id) {
    $('#sumpricegoods' + id + '').empty()
    var amount = $('#amount' + id + '').val();
    var price = $('#price' + id + '').val();
    var sum = amount * price;
    $('#sumpricegoods' + id + '').append(sum)
    Tong();
}

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

$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        Good(barcode)

    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});

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
    var partialpay = $('#partialpay').val().trim().substring(1).replace(/,/g, '')
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
    if (customerKH.length <= 0) {
        alert("Chọn khách hàng !!!")
        return;
    }
    if (user.length <= 0) {
        alert("Chọn Nhân Viên !!!")
        return;
    } if (paymethod.length <= 0) {
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
                    var discount = $('#' + id + ' #discount' + id + '').val()
                    var tax = $('#' + id + ' #tax' + id + '').val()
                    var pricediscount = $('#' + id + ' #pricediscount' + id + '').text();
                    var pricetax = $('#' + id + ' #pricetax' + id + '').text();
                    var sumpricegoods = $('#' + id + ' #sumpricegoods' + id + '').text();
                    Xuat(id, H, amount, price, discount, tax, pricediscount, pricetax, sumpricegoods)
                }

            } 
            else {
                alert("Thất bại")
            }
        },
    })
}
function Xuat(id, H, amount, price, discount, tax, pricediscount, pricetax, sumpricegoods) {
    $.ajax({
        url: '/saleorder/AddDetails',
        type: 'post',
        data: {
            id, H, amount, price, discount, tax, pricediscount, pricetax, sumpricegoods
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
                    $('span[name="sumpricedh"]').append(v.datepay)
                    $('span[name="Tong"]').append(v.sumprice)
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
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td>' + v.amount + '</td>'
                    table += '<td>' + v.price + '</td>'
                    table += '<td >' + v.discount + '</td>'
                    table += '<td class="modaldiscount">' + v.pricediscount + '</td>'
                    table += '<td>' + v.tax + '</td>'
                    table += '<td class="modaltax">' + v.pricetax + '</td>'
                    table += '<td class="modalsumprice">' + v.sumprice + '</td></tr>'
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



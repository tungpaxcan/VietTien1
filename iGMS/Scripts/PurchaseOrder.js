var d = new Date();
var seach = '';
var checkboxesChecked = [];
//Chọn Nơi Nhập

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

//End Chọn Nơi Nhập

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Chọn Nhà Cung Cấp

$('#supplier').change(function () {
    var supplier = $("#supplier option:selected").val();
    if (supplier == 0) {
        $('#Excel').attr('hidden', false)
        $('#addex').attr('hidden', false)
        $('#add').attr('hidden', true)
    } else {
        $('#Excel').attr('hidden', true)
        $('#addex').attr('hidden', true)
        $('#add').attr('hidden', false)
    }
    $('#listgoods').modal('show')
    ListGoods(supplier, seach, checkboxesChecked)
})

//End Chọn Nhà Cung Cấp

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Tìm Kiếm Sản Phẩm

$('#seachidgood').keyup(function () {
    var supplier = $("#supplier option:selected").val();
    seach = $('#seachidgood').val().trim();
    var checkboxes = document.getElementsByName('change');
    // loop over them all
    for (var i = 0; i < checkboxes.length; i++) {
        // And stick the checked ones onto an array...
        /*      if ($('#seachidgood').val().trim().length <= 1) {*/
        if (checkboxes[i].checked) {
            if (checkboxesChecked.includes(checkboxes[i].value) == false) {
                checkboxesChecked.push(checkboxes[i].value);
            }
            else {
            }
        }
    }
    ListGoods(supplier, seach, checkboxesChecked)
})

//END Tìm Kiếm Sản Phẩm

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Hiển Thị Sản Phẩm Mua

function ListGoods(supplier, seach, checkboxesChecked) {
    $.ajax({
        url: '/purchaseorder/ListGoods',
        type: 'get',
        data: { supplier, seach },
        success: function (data) {
            var Stt = 1;
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    var ids = $('.nhanhang').map(function () {
                        return this.id;
                    }).get();
                    if (ids.includes(v.idgood)) {
                    }
                    else {
                        let table = '<tr id="' + v.idgood + '" role="row" class="odd nhanhang">';
                        table += '<td class="datatable-cell-sorted datatable-cell-center datatable-cell datatable-cell-check" data-field="RecordID" aria-label="2"><span style="width: 30px;"><label class="checkbox checkbox-single kt-checkbox--solid"><input id="' + v.idgood + 'abc" onclick="Sumprice();" type="checkbox" value="' + v.idgood + '" name="change">&nbsp;<span></span></label></span></td>'
                        table += '<td>' + (Stt++) + '</td>'
                        table += '<td>' + v.idgood + '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td><input type="number" value="0" id="amount' + v.idgood + '" /></td>'
                        table += '<td>'
                        table += '<textarea name="tags-outside" placeholder="Nhập Đủ Mã EPC" class="tagify--outside form-control" id="epc' + v.idgood + '"></textarea>'
                        table += '</td>'
                        table += '<td><input type="text" value="' + v.purchaseprice + '" id="price' + v.idgood + '" /></td>'
                        table += '<td id="sumpricegoods' + v.idgood + '"></td>'
                        table += '</tr>';
                        $('#tbd').append(table);
                        PriceDiscount(v.idgood)
                        $('#amount' + v.idgood + '').keyup(function () {
                            PriceDiscount(v.idgood)
                            var amount = $('#amount' + v.idgood + '').val().trim();
                            EPC(v.idgood, amount)
                        })
                        $('#price' + v.id + '').keyup(function () {
                            PriceDiscount(v.idgood)
                        })
                    }                         
                });
                Active(checkboxesChecked);
                $('input[name="change"]').click(function () {
                    var id = $(this).closest('tr').attr('id')
                    var change = $(this).is(":checked");
                    if (change == false) {
                        checkboxesChecked.splice(checkboxesChecked.indexOf('' + id + ''), 1)
                    }
                })
               
            } else (
                alert(data.msg)
            )
        }
    })
}

//END Hiển Thị Sản Phẩm Mua.

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Trừ Serial Đã Có Trong HỆ Thống

function EPC(id, amount) {
    var input = document.getElementById('epc' + id + '');
    $.ajax({
        url: '/purchaseorder/EPCDaCo',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                let a = ''
                $.each(data.c, function (k, v) {
                    a += v.epc + ","
                })
                $.each(data.d, function (k, v) {
                    a += v.epc + ","
                })
                const myArray = a.split(",");
                new Tagify(input, {
                    maxTags: Number(amount),
                    blacklist: myArray
                })
            }
        }
    })
}

//END Trừ Serial Đã Có Trong Hệ Thống

//-------------------------------------------------------------------------------------------------------------------------------------------------
 
//---------lap cac san pham da dc tich khi tim kiem

function Active(checkboxesChecked) {
    for (var i = 0; i < checkboxesChecked.length; i++) {
        var good = checkboxesChecked[i]
        $('#' + good + 'abc').attr('checked', true)
    }
}

//Tích Và Bỏ Tích Tất Cả

$('#allchange').click(function () { 
    var allchange = $('#allchange').is(":checked"); 
    if (allchange == false) {
        $('input[name="change"]').attr('checked', false)
        Sumprice()
    }
    else {
        $('input[name="change"]').attr('checked', true)
        Sumprice()
    }
})

//End Tích Và Bỏ Tích Tất Cả

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Tổng Tiền Trên Sản Phẩm

function PriceDiscount(id) {
    $('#sumpricegoods' + id + '').empty();
    var amount = $('#amount' + id + '').val().trim();
    var price = $('#price' + id + '').val().trim();
    var prices = (Number(amount) * Number(price))
    $('#sumpricegoods' + id + '').append(prices)
    Sumprice();
}

//Tổng Tiền Trên Sản Phẩm

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Tổng Tiền Của Đơn Hàng

function Sumprice() {
    $('#sumprice').empty();
    var checkboxes = document.getElementsByName('change');
    var checkboxesChecked = [];
    var sumpricegoods = 0;
    // loop over them all
    for (var i = 0; i < checkboxes.length; i++) {
        // And stick the checked ones onto an array...
        if (checkboxes[i].checked) {
            checkboxesChecked.push(checkboxes[i]);
            var a = document.getElementById('sumpricegoods' + checkboxes[i].value + '').innerText;
            sumpricegoods += Number(a);
        }
    }
    $('#sumprice').append(sumpricegoods)
    Liabilities()
}

//END Tổng Tiền Của Đơn Hàng

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Số Tiền Đã Trả Trước

$('#partialpay').keyup(function () {
    Liabilities()
})

//END Nhập Số Tiền Đã Trả Trước

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Tiền còn nợ

function Liabilities() {
    $('#liabilities').empty();
    var partialpay = $('#partialpay').val().trim()
    var sumprice = document.getElementById('sumprice').innerText;
    $('#liabilities').append(Number(sumprice) - Number(partialpay))
}

//END Tiền còn nợ

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Thêm ĐƠn Hàng Mua Vào Cơ Sở Dữ Liệu Bằng Nhập tay

function Add() {
    var name = $('#name').val().trim();
    var paymethod = $("#paymethod option:selected").val();
    var supplier = $("#supplier option:selected").val();
    var datepay = $("#datepay").val().trim();
    var deliverydate = $("#deliverydate").val().trim();
    var sumprice = document.getElementById("sumprice").innerText;
    var partialpay = $("#partialpay").val().trim();
    var liabilities = document.getElementById("liabilities").innerText;
    var des = $("#des").val().trim()
    var  H= ""
    if ($("#warehouse option:selected").val() == -1) {
        H = $("#store option:selected").val()

    } else {
        H = $("#warehouse option:selected").val()
    }
    if (name.length <= 0) {
        alert("Nhập Tên Đơn Hàng!!!")
        return;
    }
    if (H == -1) {
        alert("Chọn Nơi Nhập Hàng !!!")
        return;
    }
    if (supplier == -1) {
        alert("Chọn Nhà Cung Cấp!!!")
        return;
    } if (paymethod == -1) {
        alert("Chọn Phương Thức Thanh Toán!!!")
        return;
    } if (datepay.length <= 0) {
        alert("Chọn Thời Hạn Thanh Toán!!!")
        return;
    } if (deliverydate.length <= 0) {
        alert("Chọn Thời Hạn Thanh Toán!!!")
        return;
    }
    if (partialpay.length <= 0) {
        alert("Nhập Tiền Đã Thanh Toán!!!")
        return;
    }
    $.ajax({
        url: '/purchaseorder/Add',
        type: 'post',
        data: {
            paymethod, name, datepay, deliverydate, sumprice, liabilities, partialpay, des,H,supplier
        },
        success: function (data) {
            if (data.code == 200) {
                var checkboxes = document.getElementsByName('change');
                var checkboxesChecked = [];
                // loop over them all
                for (var i = 0; i < checkboxes.length; i++) {
                    // And stick the checked ones onto an array...
                    if (checkboxes[i].checked) {
                        checkboxesChecked.push(checkboxes[i].value);
                        var amount = $('#amount' + checkboxes[i].value + '').val().trim() 
                        var goods = checkboxes[i].value;
                        var price = $('#price' + checkboxes[i].value + '').val().trim()
                        var sumpricegoods = document.getElementById('sumpricegoods' + checkboxes[i].value + '').innerText;
                        if (amount == 0) {
                            alert("Chưa Nhập Số lượng  Cho " + goods + "!!!")
                            return;
                        }
                        var tags =JSON.parse($('#epc' + goods + '').val())
                        var TagArray = [];
                        //Convert to array
                        for (let j = 0; j < tags.length; j++) {
                            TagArray.push(tags[j].value)
                        }                                         
                        for (let k = 0; k < TagArray.length; k++) {
                            var epc = TagArray[k]

                            if (TagArray.length < amount) {
                                alert("Chưa Nhập Đủ Mã EPC Cho " + goods + " !!!")
                                return;
                            }
                            $.ajax({
                                url: '/purchaseorder/AddDetail',
                                type: 'post',
                                data: {
                                    price, sumpricegoods, epc, goods, H
                                },
                                success: function (data) {
                                    if (data.code == 200) {
                                        BILL()
                                    } 
                                    else {
                                    }
                                },
                            })
                        }                           
                    }
                }               
            } 
            else {
                alert("Tạo Đơn Hàng Thất Bại")
            }
        }
    })
}

//END Thêm Đơn Hàng Mua Vào Cơ Sở Dữ Liệu Bằng Nhập tay

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Truy xuất dữ Liệu Từ File Excel

var input = document.getElementById('fileUpload');
input.addEventListener('change', function () {
    readXlsxFile(input.files[0]).then(function (rows) {
        var Stt = 1;
        for (let i = 1; i < rows.length; i++) {         
            var id = rows[i][0];
            let table = '<tr id="' + id + '" role="row" class="odd nhanhangexcel">';
            table += '<td class="datatable-cell-sorted datatable-cell-center datatable-cell datatable-cell-check" data-field="RecordID" aria-label="2"><span style="width: 30px;"><label class="checkbox checkbox-single kt-checkbox--solid"><input id="' + id + 'abc" onclick="Sumprice();" type="checkbox" value="' + id + '" name="change">&nbsp;<span></span></label></span></td>'
            table += '<td>' + (Stt++) + '</td>'
            table += '<td>' + rows[i][2] + '</td>'
            table += '<td>' + rows[i][3] + '</td>'
            table += '<td ><input disabled value="1" id="amount' + id + '" / ></td>'
            table += '<td id="mahang">' + rows[i][1] + '</td>'
            table += '<td><input disabled type="text" value="' + rows[i][4] + '" id="price' + id + '" /></td>'
            table += '<td id="sumpricegoods' + id + '"></td>'
            table += '</tr>';
            $('#tbd').append(table);
            PriceDiscount(id)
            Sumprice()
        }

    })
})

//Truy xuất dữ Liệu Từ File Excel

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Thêm Đơn Hàng Mua Vào Cơ Sở Dữ Liệu Bằng Excel

function AddEX() {
    var name = $('#name').val().trim();
    var paymethod = $("#paymethod option:selected").val();
    var supplier = $("#supplier option:selected").val();
    var datepay = $("#datepay").val().trim();
    var deliverydate = $("#deliverydate").val().trim();
    var sumprice = document.getElementById("sumprice").innerText;
    var partialpay = $("#partialpay").val().trim();
    var liabilities = document.getElementById("liabilities").innerText;
    var goods = document.getElementById("mahang").innerText;
    var des = $("#des").val().trim()
    var H = ""
    if ($("#warehouse option:selected").val() == -1) {
        H = $("#store option:selected").val()

    } else {
        H = $("#warehouse option:selected").val()
    }
    if (name.length <= 0) {
        alert("Nhập Tên Đơn Hàng!!!")
        return;
    }
    if (H == -1) {
        alert("Chọn Nơi Nhập Hàng !!!")
        return;
    }
    if (supplier == -1) {
        alert("Chọn Nhà Cung Cấp!!!")
        return;
    } if (paymethod == -1) {
        alert("Chọn Phương Thức Thanh Toán!!!")
        return;
    } if (datepay.length <= 0) {
        alert("Chọn Thời Hạn Thanh Toán!!!")
        return;
    } if (deliverydate.length <= 0) {
        alert("Chọn Thời Hạn Thanh Toán!!!")
        return;
    }
    if (partialpay.length <= 0) {
        alert("Nhập Tiền Đã Thanh Toán!!!")
        return;
    }
    $.ajax({
        url: '/purchaseorder/Add',
        type: 'post',
        data: {
            paymethod, name, datepay, deliverydate, sumprice, liabilities, partialpay, des, H, supplier
        },
        success: function (data) {
            if (data.code == 200) {
                var checkboxes = document.getElementsByName('change');
                var checkboxesChecked = [];
                // loop over them all
                for (var i = 0; i < checkboxes.length; i++) {
                    // And stick the checked ones onto an array...
                    if (checkboxes[i].checked) {
                        checkboxesChecked.push(checkboxes[i].value);
                        var amount = $('#amount' + checkboxes[i].value + '').val().trim()
                        var epc = checkboxes[i].value;
                        var price = $('#price' + checkboxes[i].value + '').val().trim()
                        var sumpricegoods = document.getElementById('sumpricegoods' + checkboxes[i].value + '').innerText;
                        if (amount == 0) {
                            alert("Chưa Nhập Số lượng  Cho " + goods + "!!!")
                            return;
                        }
                            $.ajax({
                                url: '/purchaseorder/AddDetail',
                                type: 'post',
                                data: {
                                    price, sumpricegoods, epc, goods, H
                                },
                                success: function (data) {
                                    if (data.code == 200) {
                                        BILL()
                                    }
                                    else {
                                    }
                                },
                            })
                    }
                }
            }
            else {
                alert("Tạo Quầy Bán Thất Bại")
            }
        }
    })
}

//END Thêm Đơn Hàng Mua Vào Cơ Sở Dữ Liệu Bằng Excel

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Xuất Thông Tin Mua Hàng

function BILL() {
    var supplier = $("#supplier option:selected").val();
    $.ajax({
        url: '/purchaseorder/Bill',
        type: 'get',
        data: {
            supplier
        },
        success: function (data) {
            $('span[name="namesupplier"]').empty()
            $('span[name="addresssupplier"]').empty()
            $('span[name="phonesupplier"]').empty()
            $('span[name="faxsupplier"]').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    $('span[name="namesupplier"]').append(v.name)
                    $('span[name="addresssupplier"]').append(v.address)
                    $('span[name="phonesupplier"]').append(v.phone)
                    $('span[name="faxsupplier"]').append(v.fax)
                    $.ajax({
                        url: '/purchaseorder/Bill1',
                        type: 'get',
                        success: function (data) {
                            if (data.code == 200) {
                                $('span[name="iddh"]').empty()
                                $('span[name="datedh"]').empty()
                                $('span[name="paydh"]').empty()
                                $('span[name="datepaydh"]').empty()
                                $('span[name="sumpricedh"]').empty()
                                $('span[name="Tong"]').empty();
                                $('#qrcode').empty();
                                $.each(data.c, function (k, v) {
                                    $('span[name="iddh"]').append("00000" + v.id)
                                    $('span[name="datedh"]').append(v.datedh)
                                    $('span[name="paydh"]').append(v.paydh)
                                    $('span[name="datepaydh"]').append(v.datepaydh)
                                    $('span[name="sumpricedh"]').append(v.sumpricedh)
                                    $('span[name="Tong"]').append(to_vietnamese(v.sumpricedh))
                                    new QRCode(document.getElementById("qrcode"), {
                                        text: '00000' + v.id,
                                        width: 100,
                                        height: 100,

                                    })
                                })

                                $.ajax({
                                    url: '/purchaseorder/Bill2',
                                    type: 'get',
                                    success: function (data) {
                                        if (data.code == 200) {
                                            var Stt = 1;
                                            $('#tbdmodal').empty()
                                            $('span[name="sumpricediscount"]').empty()
                                            $('span[name="sumpricetax"]').empty()

                                            $.each(data.c, function (k, v) {
                                                let a = '<tr>';
                                                a += '<td>' + (Stt++) + '</td>';
                                                a += '<td>' + v.id + '</td>';
                                                a += '<td>' + v.idgood + '</td>';
                                                a += '<td>' + v.epc + '</td>';
                                                a += '<td>' + v.name + '</td>';
                                                a += '<td>' + v.price + '</td>';
                                                a += '<td>' + v.price + '</td></tr>';
                                                $('#tbdmodal').append(a)
                                            })
                                            const myTimeout = setTimeout(function () { $('#BILL').modal('show') }, 500)

                                        }
                                        else {
                                            alert("Tạo Đơn Vị Thất Bại")
                                        }
                                    }
                                })
                            }
                            else {
                                alert("Tạo Đơn Vị Thất Bại")
                            }
                        }
                    })
                })

            }
            else {
                alert("Tạo Đơn Vị Thất Bại")
            }
        }
    })

}

//End Xuất Thông Tin Mua Hàng

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Chọn Kho Hàng Xuất

$('#XuatTuKho').change(function () {
    var warehouse = $('#XuatTuKho option:selected').val();
    ListGoods1(warehouse, seach)
})

//END Chọn Kho Hàng Xuất

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Tìm KIếm Hàng Từ kho Xuất

$('#seachidgood1').keyup(function () {
    var seach = $('#seachidgood1').val().trim()
    var warehouse = $('#XuatTuKho option:selected').val();
    var checkboxes = document.getElementsByName('change');
    // loop over them all
    for (var i = 0; i < checkboxes.length; i++) {
        // And stick the checked ones onto an array...
        /*      if ($('#seachidgood').val().trim().length <= 1) {*/
        if (checkboxes[i].checked) {
            if (checkboxesChecked.includes(checkboxes[i].value) == false) {
                checkboxesChecked.push(checkboxes[i].value);
            }
            else {
            }
        }
    }
    ListGoods1(warehouse, seach, checkboxesChecked)
})

//Hiển Thị Sản Phẩm Đề Xuất Thêm Hàng

//-------------------------------------------------------------------------------------------------------------------------------------------------

function ListGoods1(warehouse, seach, checkboxesChecked) {
    $.ajax({
        url: '/purchaseorder/ListGoods1',
        type: 'get',
        data: { warehouse, seach },
        success: function (data) {
            var Stt = 1;
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    var ids = $('.nhanhang').map(function () {
                        return this.id;
                    }).get();
                    if (ids.includes(v.idgood)) {
                        var amount = $('#amount' + v.idgood + '').val().trim()
                        $('#amount' + v.idgood + '').empty()
                        $('#amount' + v.idgood + '').val(Number(amount) + 1)
                    }
                    else {
                        let table = '<tr id="' + v.idgood + '" role="row" class="odd nhanhang">';
                        table += '<td class="datatable-cell-sorted datatable-cell-center datatable-cell datatable-cell-check" data-field="RecordID" aria-label="2"><span style="width: 30px;"><label class="checkbox checkbox-single kt-checkbox--solid"><input id="' + v.idgood + 'abc" onclick="Sumprice();" type="checkbox" value="' + v.idgood + '" name="change">&nbsp;<span></span></label></span></td>'
                        table += '<td>' + (Stt++) + '</td>'
                        table += '<td>' + v.idgood + '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td><input type="number" value="1" id="amount' + v.idgood + '" /></td>'
                        table += '</tr>';
                        $('#tbd').append(table);
                        $('#amount' + v.idgood + '').keypress(function (e) {
                            if (e.which == 13) {
                                var amount = $(this).val().trim();
                                ValidateAmount(warehouse, v.idgood, amount)
                            }                           
                        })
                    }
                });
                Active(checkboxesChecked)
                $('input[name="change"]').click(function () {
                    var id = $(this).closest('tr').attr('id')
                    var change = $(this).is(":checked");
                    if (change == false) {
                        checkboxesChecked.splice(checkboxesChecked.indexOf('' + id + ''), 1)
                    }
                })
            } else (
                alert(data.msg)
            )
        }
    })
}

//END Hiển Thị Sản Phẩm Mua.

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Kiểm Tra Số Lượng Hàng Tồn Còn ở Kho

function ValidateAmount(warehouse,idgood,amount) {
    $.ajax({
        url: '/purchaseorder/ValidateAmount',
        type: 'get',
        data: { warehouse, idgood },
        success: function (data) {
            if (data.code == 200) {
                if (data.c < amount) {
                    alert("Hàng Không Đủ Số Lượng, Chỉ Còn " + data.c + "")
                    $('#amount' + idgood + '').val(data.c)
                }
            } else (
                alert(data.msg)
            )
        }
    })
}

//END Kiểm Tra Số Lượng Hàng Tồn Còn ở Kho

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Thêm Đơn Hàng Cần Nhập Vào Cơ Sở Dữ Liệu Bằng Nhập tay

function Add1() {
    var name = $('#name').val().trim();
    var des = $("#des").val().trim()
    var H = ""
    if ($("#warehouse option:selected").val() == -1) {
        H = $("#store option:selected").val()

    } else {
        H = $("#warehouse option:selected").val()
    }
    if (name.length <= 0) {
        alert("Nhập Tên Đơn Hàng!!!")
        return;
    }
    if (H == -1) {
        alert("Chọn Nơi Nhập Hàng !!!")
        return;
    }
    $.ajax({
        url: '/purchaseorder/Add1',
        type: 'post',
        data: {
             name, des, H,
        },
        success: function (data) {
            if (data.code == 200) {
                var checkboxes = document.getElementsByName('change');
                var checkboxesChecked = [];
                // loop over them all
                for (var i = 0; i < checkboxes.length; i++) {
                    // And stick the checked ones onto an array...
                    if (checkboxes[i].checked) {
                        checkboxesChecked.push(checkboxes[i].value);
                        var amount = $('#amount' + checkboxes[i].value + '').val().trim()
                        var goods = checkboxes[i].value;
                            $.ajax({
                                url: '/purchaseorder/AddDetail1',
                                type: 'post',
                                data: {
                                    goods, H, amount
                                },
                                success: function (data) {
                                    if (data.code == 200) {
                                        BILL1()
                                    }
                                    else {
                                    }
                                },
                            })
                    }
                }
            }
            else {
                alert("Tạo Đơn Hàng Thất Bại")
            }
        }
    })
}

//END Thêm Đơn Hàng Mua Vào Cơ Sở Dữ Liệu Bằng Nhập tay

//-------------------------------------------------------------------------------------------------------------------------------------------------

//Xuất Thông Tin Mua Hàng

function BILL1() {
    var warehouse = $("#XuatTuKho option:selected").val();
    $.ajax({
        url: '/purchaseorder/Bill1',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                $('span[name="iddh"]').empty()
                $('span[name="datedh"]').empty()
                $('#qrcode').empty();
                $('span[name="namewarehouse"]').empty()
                $('span[name="namewarehouse"]').append(warehouse)
                $.each(data.c, function (k, v) {
                    $('span[name="iddh"]').append("00000" + v.id)
                    $('span[name="datedh"]').append(v.datedh)
                    new QRCode(document.getElementById("qrcode"), {
                        text: '00000' + v.id,
                        width: 100,
                        height: 100,

                    })
                })
                $.ajax({
                    url: '/purchaseorder/Bill2',
                    type: 'get',
                    success: function (data) {
                        if (data.code == 200) {
                            var Stt = 1;
                            $('#tbdmodal').empty()
                            $.each(data.d, function (k, v) {
                                var ids = $('.Bill').map(function () {
                                    return this.id
                                }).get()
                                if (ids.includes(v.idgood)) {

                                } else {
                                    let a = '<tr class="Bill" id="' + v.idgood + '">';
                                    a += '<td>' + (Stt++) + '</td>';
                                    a += '<td>' + v.idgood + '</td>';
                                    a += '<td>' + v.name + '</td>';
                                    a += '<td>' + v.amount + '</td></tr>';
                                    $('#tbdmodal').append(a)
                                }                
                            })
                            const myTimeout = setTimeout(function () { $('#BILL').modal('show') }, 500)
                        }
                        else {
                            alert("Lỗi !!!")
                        }
                    }
                })
            }
            else {
                alert("Lỗi !!!")
            }
        }
    })
}

//End Xuất Thông Tin Mua Hàng
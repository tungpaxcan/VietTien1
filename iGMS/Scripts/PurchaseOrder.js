var d = new Date();
var seach = '';
var checkboxesChecked = [];
//----------Change::supplier--------------
$('#supplier').change(function () {
    var supplier = $("#supplier option:selected").val();
    $('#listgoods').modal('show')
    ListGoods(supplier, seach, checkboxesChecked)

})

//------------tim kiem-------------------
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
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td class="datatable-cell-sorted datatable-cell-center datatable-cell datatable-cell-check" data-field="RecordID" aria-label="2"><span style="width: 30px;"><label class="checkbox checkbox-single kt-checkbox--solid"><input id="'+v.id+'abc" onclick="Sumprice();" type="checkbox" value="' + v.id + '" name="change">&nbsp;<span></span></label></span></td>'
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td><input type="number" value="1" id="amount' + v.id + '" /></td>'
                    table += '<td><input type="text" value="' + v.purchaseprice+'" id="price' + v.id + '" /></td>'
                    table += '<td><input type="number" value="' + v.purchasediscount+'" id="discount' + v.id + '" placeholder="%" /></td>'
                    table += '<td id="pricediscount' + v.id + '"></td>'
                    table += '<td><input type="number" value="' + v.purchasetax+'" id="tax' + v.id + '" /></td>'
                    table += '<td id="pricetax' + v.id + '"></td>'
                    table += '<td id="sumpricegoods' + v.id + '"></td>'
                  
                    table += '</tr>';
                    $('#tbd').append(table);
                    PriceDiscount(v.id)
                    $('#amount' + v.id + '').keyup(function () {
                        PriceDiscount(v.id)
                    })
                    $('#price' + v.id + '').keyup(function () {
                        PriceDiscount(v.id)
                        })
                    $('#discount' + v.id + '').keyup(function () {
                        PriceDiscount(v.id)
                    })
                    $('#tax' + v.id + '').keyup(function () {
                        PriceTax(v.id)
                    })
               
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

//---------lap cac san pham da dc tich khi tim kiem

function Active(checkboxesChecked) {

    for (var i = 0; i < checkboxesChecked.length; i++) {
        var good = checkboxesChecked[i]
        $('#' + good + 'abc').attr('checked', true)
    }
}


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

function PriceDiscount(id) {
    $('#pricediscount' + id + '').empty();
    $('#pricetax' + id + '').empty();
    $('#sumpricegoods' + id + '').empty();
    var amount = $('#amount' + id + '').val().trim();
    var price = $('#price' + id + '').val().trim().includes(",") == true ? $('#price' + id + '').val().trim().substring(1).replace(/,/g, "") : $('#price' + id + '').val().trim();
    var discount = $('#discount' + id + '').val().trim();
    var tax = $('#tax' + id + '').val().trim();
    var prices = (Number(amount) * Number(price))
    var pricediscount = prices * (Number(discount) / 100)
    var pricetax = prices * (Number(tax) / 100)
    $('#pricediscount' + id + '').append(pricediscount)
    $('#pricetax' + id + '').append(pricetax)
    $('#sumpricegoods' + id + '').append(prices - pricediscount + pricetax)
    Sumprice();


}
function PriceTax(id) {
    $('#pricetax' + id + '').empty();
    $('#sumpricegoods' + id + '').empty();
    var amount = $('#amount' + id + '').val().trim();
    var price = $('#price' + id + '').val().trim().includes(",") == true ? $('#price' + id + '').val().trim().substring(1).replace(/,/g, "") : $('#price' + id + '').val().trim();
    var discount = $('#discount' + id + '').val().trim();
    var tax = $('#tax' + id + '').val().trim();
    var prices = (Number(amount) * Number(price))
    var pricediscount = prices * (Number(discount) / 100)
    var pricetax = prices * (Number(tax) / 100)

    var pricetax = prices * (Number(tax) / 100)
    $('#pricetax' + id + '').append(pricetax)
    $('#sumpricegoods' + id + '').append(prices - pricediscount + pricetax)
    $('#sumprice').append(prices - pricediscount + pricetax)
    Sumprice()

}


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
    $('#sumprice').append(Money(sumpricegoods))
    Liabilities()
}

$('#partialpay').keyup(function () {
    Liabilities()
})

function Liabilities() {
    $('#liabilities').empty();
    var partialpay = $('#partialpay').val().trim().includes(",") == true ? $('#partialpay').val().trim().substring(1).replace(/,/g, "") : $('#partialpay').val().trim()
    var sumprice = document.getElementById('sumprice').innerText.substring(1).replace(/,/g, "");
    $('#liabilities').append(Money(Number(sumprice) - Number(partialpay)))
}

//----------------Add::PurchaseOrder---------------------
function Add() {
    var name = $('#name').val().trim();
    var paymethod = $("#paymethod option:selected").val();
    var supplier = $("#supplier option:selected").val();
    var datepay = $("#datepay").val().trim();
    var deliverydate = $("#deliverydate").val().trim();
    var sumprice = document.getElementById("sumprice").innerText.substring(1).replace(/,/g, "");
    var partialpay = $("#partialpay").val().trim().substring(1).replace(/,/g, "");
    var liabilities = document.getElementById("liabilities").innerText.substring(1).replace(/,/g, "");
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
                        var price = $('#price' + checkboxes[i].value + '').val().trim().substring(1).replace(/,/g, "")
                        var discount = $('#discount' + checkboxes[i].value + '').val().trim()
                        var pricediscount = document.getElementById('pricediscount' + checkboxes[i].value + '').innerText;
                        var tax = $('#tax' + checkboxes[i].value + '').val().trim()
                        var pricetax = document.getElementById('pricetax' + checkboxes[i].value + '').innerText;
                        var sumpricegoods = document.getElementById('sumpricegoods' + checkboxes[i].value + '').innerText;
                            $.ajax({
                                url: '/purchaseorder/AddDetail',
                                type: 'post',
                                data: {
                                    amount, goods, price, discount, pricediscount, tax, pricetax, sumpricegoods
                                },
                                success: function (data) {
                                    if (data.code == 200) {
                                        
                                    }
                                    else {
                                        alert("Tạo Đơn Vị Thất Bại")
                                    }
                                },
                            })
                        $.ajax({
                            url: '/purchaseorder/EditDetailSupplierGoods',
                            type: 'post',
                            data: {
                                supplier, goods, price, tax, discount
                            },
                            success: function (data) {
                                if (data.code == 200) {
                                    if (i == checkboxes.length) {
                                        BILL()
                                    }
                                } 
                                else {
                                    alert("Tạo Đơn Hàng Thất Bại")
                                }
                            },
                        })
                    }
                }
              
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Tạo Quầy Bán Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}
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
                                            a += '<td>' + v.idgoods + '</td>';
                                            a += '<td>' + v.name + '</td>';
                                            a += '<td>' + v.unit + '</td>';
                                            a += '<td>' + v.amount + '</td>';
                                            a += '<td>' + v.price + '</td>';
                                            a += '<td>' + v.discount + '</td>';
                                            a += '<td name="pricediscount1" id="' + v.pricediscount + '">' + v.pricediscount + '</td>';
                                            a += '<td>' + v.tax + '</td>';
                                            a += '<td name="pricetax1" id="' + v.pricetax + '">' + v.pricetax + '</td>';
                                            a += '<td>' + v.sumprice + '</td></tr>';
                                            $('#tbdmodal').append(a)
                                        })
                                        var sum = $('td[name="pricediscount1"]').map(function (_, x) { return x.id; }).get();
                                        var sum1 = $('td[name="pricetax1"]').map(function (_, x) { return x.id; }).get();
                                        var sums = 0
                                        var sums1 = 0
                                        for (var i = 0; i < sum.length; i++) {
                                            sums += (Number(sum[i]))
                                        }
                                        for (var i = 0; i < sum1.length; i++) {
                                            sums1 += (Number(sum1[i]))
                                        }
                                        $('span[name="sumpricediscount"]').append(sums)
                                        $('span[name="sumpricetax"]').append(sums1)                                    
                                        $('#BILL').modal('show')
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

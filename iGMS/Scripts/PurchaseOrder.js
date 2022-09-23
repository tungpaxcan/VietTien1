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
                    table += '<td>' + v.size + '</td>'
                    table += '<td><input type="number" value="0" id="amount' + v.id + '" /></td>'
                    table += '<td>'
                    table += '<textarea name="tags-outside" placeholder="Nhập Đủ Mã EPC" class="tagify--outside form-control" id="epc' + v.id + '"></textarea>'
                    table += '</td>'
                    table += '<td><input type="text" value="' + v.purchaseprice+'" id="price' + v.id + '" /></td>'
                    table += '<td id="sumpricegoods' + v.id + '"></td>'                 
                    table += '</tr>';
                    $('#tbd').append(table);
                    PriceDiscount(v.id)
                    $('#amount' + v.id + '').keyup(function () {
                      
                        PriceDiscount(v.id)
                        var amount = $('#amount' + v.id + '').val().trim();
                        EPC(v.id, amount)
                    })
                    $('#price' + v.id + '').keyup(function () {
                        PriceDiscount(v.id)
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
    $('#sumpricegoods' + id + '').empty();
    var amount = $('#amount' + id + '').val().trim();
    var price = $('#price' + id + '').val().trim();
    var prices = (Number(amount) * Number(price))
    $('#sumpricegoods' + id + '').append(prices)
    Sumprice();
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
    $('#sumprice').append(sumpricegoods)
    Liabilities()
}

$('#partialpay').keyup(function () {
    Liabilities()
})

function Liabilities() {
    $('#liabilities').empty();
    var partialpay = $('#partialpay').val().trim()
    var sumprice = document.getElementById('sumprice').innerText;
    $('#liabilities').append(Number(sumprice) - Number(partialpay))
}

//----------------Add::PurchaseOrder---------------------
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
                                    goods, price, sumpricegoods, epc
                                },
                                success: function (data) {
                                    if (data.code == 200) {

                                    } 
                                    else {
                                    }
                                },
                            })
                        }                           
                        $.ajax({
                            url: '/purchaseorder/EditDetailSupplierGoods',
                            type: 'post',
                            data: {
                                supplier, goods, price,
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
                                            var tags = JSON.parse($('#epc' + v.idgoods + '').val());
                                            var TagArray = [];
                                            //Convert to array
                                            for (let i = 0; i < tags.length; i++) {
                                                TagArray.push(tags[i].value)
                                            }
                                            let a = '<tr>';
                                            a += '<td>' + (Stt++) + '</td>';
                                            a += '<td>' + v.idgoods + '</td>';
                                            a += '<td>' + v.epc + '</td>';
                                            a += '<td>' + v.name + '</td>';
                                            a += '<td>' + v.size + '</td>';
                                            a += '<td>' + v.price + '</td>';
                                            a += '<td>' + v.sumprice + '</td></tr>';
                                            $('#tbdmodal').append(a)
                                        })
                                        const myTimeout = setTimeout(function () { $('#BILL').modal('show')}, 500)
                                       
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
function EPC(id,amount){
    var input = document.getElementById('epc' + id + '');
    // init Tagify script on the above inputs
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

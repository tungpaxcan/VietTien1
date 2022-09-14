$('input[name="purchaseorder"]').keyup(function () {
    var purchaseorder = $('input[name="purchaseorder"] ').val();
    var id = $('#id').val().trim();
    var d = new Date()
    $.ajax({
        url: '/Receipt/DetailGoodOrder',
        type: 'get',
        data: {
            purchaseorder
        },
        success: function (data) {
            $('span[name="paydh"]').empty()
            $('span[name="datedh"]').empty()
            $('#datepaydh').empty()
            $('span[name="namesupplier"]').empty()
            $('span[name="addresssupplier"]').empty()
            $('span[name="phonesupplier"]').empty()
            $('span[name="faxsupplier"]').empty()
            $('span[name="idpurchaseorder"]').empty()
            $('span[name="warehouse"]').empty()
            $('#sumprice').empty()
            $('tbody[name="tbd"]').empty();
            $('span[name="Tong"]').empty();
            $('#tbdct').empty();
            $('#sumpricetax').empty();
            var Stt = 1;
            var pricetax = 0;
            if (data.code == 200) {
                $.each(data.d, function (k, v) {
                    $('span[name="paydh"]').append(v.paymethod)
                    $('span[name="datedh"]').append(v.createdate)
                    $('#datepaydh').append(v.datepay)
                    $('span[name="namesupplier"]').append(v.supplier)
                    $('span[name="addresssupplier"]').append(v.addresssupplier)
                    $('span[name="phonesupplier"]').append(v.phonesupplier)
                    $('span[name="faxsupplier"]').append(v.faxsupplier)
                    $('span[name="idpurchaseorder"]').append(v.idpurchaseorder)
                    $('span[name="warehouse"]').append(v.warehouse)
                    $('#sumprice').append(Money(v.sumprice))
                    $('span[name="Tong"]').append(to_vietnamese(v.sumprice))
                  
                    
                });
                new QRCode(document.getElementById("qrcode"), {
                    text: $('#id').val().trim(),
                    width: 100,
                    height: 100,

                })
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd nhanhang">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td id="amountresult'+v.id+'">'+v.amount+'</td>'
                    table += '<td>'+v.price+'</td>'
                    table += '<td>'+v.discount+'</td>'
                    table += '<td>'+v.pricediscount+'</td>'
                    table += '<td>'+v.tax+'</td>'
                    table += '<td>'+v.pricetax+'</td>'
                    table += '<td>' + v.sumprice + '</td>'
                    table += '<td id="result' + v.id + '">0</td>'

                    table += '</tr>';
                    pricetax += Number(v.pricetax)
                    $('tbody[name="tbd"]').append(table);
                   
                });
                $.each(data.d, function (k, v) {
                    let table = '<tr role="row" class="odd">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + id + '</td>'
                    table += '<td>' + id + '</td>'
                    table += '<td>' + d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear()+'</td>'
                    table += '<td id="sumpricetax">' + pricetax+'</td>'
                    table += '<td id="sumpriceli">' + v.sumprice + '</td>'

                    table += '</tr>';
                    $('#tbdct').append(table);
                });
             
            } else (
                alert(data.msg)
            )
        }
    })
})

//-------------------add--------------------
function Add() {
    var id = $('#id').val().trim();
    var purchaseorder = $('select[name="purchaseorder"] option:selected').val();
    var method = $('#method option:selected').val();
    var user1 = $('user1 option:selected').val();
    var user2 = $('user2 option:selected').val();
    var des = $('#des').val().trim();
    if (id.length == 0) {
        alert("Nhập Phiếu Nhận !!!")
        return;
    } if (purchaseorder == -1) {
        alert("Chọn Đơn Hàng !!!")
        return;
    } if (method == -1) {
        alert("Chọn Phương Thức !!!")
        return;
    } 
    $.ajax({
        url: '/receipt/Add',
        type: 'post',
        data: {
            id, purchaseorder, user1, user2, des, method
        },
        success: function (data) {
            if (data.code == 200) {
                LastReceipt()
                $('#BILL').modal('show')
            } else if (data.code == 300) {
                alert(data.msg)
            }
        }
    })
}
function AddCT() {
    var id = $('#id').val().trim();
    var sumpricetax = $('#sumpricetax').text()
    var sumpriceli = $('#sumpriceli').text()
    $.ajax({
        url: '/receipt/AddCT',
        type: 'post',
        data: {
            id, sumpricetax, sumpriceli
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Lưu Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                printDiv('inct')
                window.location.href="/Receipt/Index"
            } else if (data.code == 300) {
                alert(data.msg)
            } else {
                alert(data.msg)
            }
        }
    })
}

//-------------LastReceip----------
function LastReceipt() {
    $.ajax({
        url: '/receipt/ReceipLast',
        type: 'get',
        success: function (data) {
            $('span[name="idpn"]').empty()
            $('span[name="datepn"]').empty()
            if (data.code == 200) {
                $('span[name="idpn"]').append(data.idpn)
                $('span[name="datepn"]').append(data.datepn)
            }
        }
    })
}

$('#btnct').click(function () {
    $('#CT').modal('show')

})


function CompareReceipt(barcode) {
    var amounttext = $('#result' + barcode + '').text();
    var amountresulttext = $('#amountresult' + barcode + '').text();
    var amount = Number(amounttext)
    var amountresult = Number(amountresulttext)
    var GoodPucharseOder = $('.tablenhan .nhanhang').map(function () {

        return this.id;
    })
    for (var i = 0; i < GoodPucharseOder.length; i++) {
        if (GoodPucharseOder[i] == barcode) {
            
            amount += 1;
            if (amount > amountresult) {
                return;
            } else {
                $('#result' + barcode + '').text(amount)
            }
            if (amount == amountresult) {
                $('#result' + barcode + '').css('background', 'green')
            } else if (amount < amountresult) {
                $('#result' + barcode + '').css('background', 'yellow')
            } else if (amount <= 0) {
                $('#result' + barcode + '').css('background', 'red')
            }
           
        }
    }
}

//-----------------------------
$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        CompareReceipt(barcode)

    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});
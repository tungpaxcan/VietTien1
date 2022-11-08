var epcshow = [];

$(document).ready(function () {
    $.ajax({
        url: '/receipt/Ready_Session',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                var Id_length = data.Id_Sales.length
                var Ids = data.Id_Sales.substring(Id_length - 4)
                var Id = data.Id_Sales.substring(0, Id_length-4);
                if (Ids == "0102") {
                    $('#idsaleorder').val('00000' + Id)
                }
                else {
                    $('#idsaleorder').val('00000' + data.Id_Sales)
                }
                Show(data.Id_Sales)
            } else (
                alert(data.msg)
            )
        }
    })
})

//Hiển thị thông tin đơn hàng để xuất

function Show(id) {
    $.ajax({
        url: '/detailwarehouse/Show2',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            $('span[name="K"]').empty()
            $('span[name="customer"]').empty()
            $('tbody[name="tbd"]').empty()
            var Stt=1
            if (data.code == 200) {
                $.each(data.a, function (k, v) {
                    $('span[name="K"]').append(v.K)
                    $('span[name="K"]').attr('id',v.Kid)
                    $('span[name="customer"]').append(v.customer)
                })
                $.each(data.c, function (k, v) {
                    var ids = $('.IDSA').map(function () {
                        return this.id;
                    }).get();
                    if (ids.includes(v.idgood)) {
                        var amounts = $('#amountresult' + v.idgood + '').text();
                        $('#amountresult' + v.idgood + '').empty();
                        $('#amountresult' + v.idgood + '').append(Number(amounts) + 1);
                    } else {
                        let table = '<tr id="' + v.idgood + '" class="donhang IDSA">'
                        table += '<td style="background:red" id="result' + v.idgood + '">0</td>'
                        table += '<td>' + (Stt++) + '</td>'
                        table += '<td hidden>' + v.idgood + '</td>'
                        table += '<td>' + v.idgood + '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td id="amountresult' + v.idgood + '">1</td></tr>'
                        $('tbody[name="tbd"]').append(table)
                    }
                })
            }  else {
                alert(data.msg)
            }
        }
    })
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//Quet hàng và so sánh với sô lượng thực nhập

function CompareReceipt(id) {
    var barcode = '';
    $.ajax({
        url: '/receipt/ChangeGood',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    barcode = v.idgood;
                    var amounttext = $('#result' + barcode + '').text();
                    var amountresulttext = $('#amountresult' + barcode + '').text();
                    var amount = Number(amounttext)
                    var amountresult = Number(amountresulttext)
                    var GoodPucharseOder = $('.donhang').map(function () {
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
                                $('#result' + barcode + '').css('background', '#ffa800')
                            } else if (amount <= 0) {
                                $('#result' + barcode + '').css('background', 'red')
                            }
                        }
                    }
                })
            } else if (data.code == 300) {
                alert(data.msg)
            }
        }
    })

}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//lưu đơn vừa xuất vào cơ sở dữ liệu

function Add() {
    var ids = $('.donhang').map(function () {
        return this.id
    }).get();
    var idsaleorder = $('#idsaleorder').val().trim().substring(5)
    var id = ""
    for (let i = 0; i < ids.length; i++) {
        id = ids[i]
        var amount = $('#result' + id + '').text()
        var K = $('span[name="K"]').attr('id');
        $.ajax({
            url: '/detailwarehouse/Tru',
            type: 'post',
            data: {
                id, idsaleorder, amount, K, epcshow
            },
            success: function (data) {
                if (data.code == 200) {
                    window.location.href = "/DetailWareHouse/Index2"
                }
                else if (data.code == 1) {
                    alert(data.msg)
                } else {
                    alert(data.msg)
                }
            }
        })
    }   
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//Quet mã hàng hóa

$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        epcshow.push(barcode)
        CompareReceipt(barcode)
    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------


var a = setInterval(function () { }, 200);
function RFID() {  a = setInterval(function () { AllShowEPC() }, 200); }

function AllShowEPC() {
    $.ajax({
        url: '/rfid/AllShowEPC',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                $.each(data.a, function (k, v) {
                    CompareReceiptrfid(v.id)
                })
            }
        }
    })
}

function StopRFID() {
    clearInterval(a)
}

function CompareReceiptrfid(epc) {
    $.ajax({
        url: '/rfid/CompareReceipt',
        type: 'get',
        data: {
            epc
        },
        success: function (data) {
            if (data.code == 200) {
                $.each(data.a, function (k, v) {
                    epcshow.push(v.idgood)
                    CompareReceipt(v.idgood)
                })
            }
        }
    })
}

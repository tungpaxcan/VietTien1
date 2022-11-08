// Chọn Nơi Xuất Hàng Bán

$.ajax({
    url: '/home/WareHouse',
    type: 'get',
    success: function (data) {
        if (data.code == 200) {
            if (data.id == -1) {

                WareHouse();
                Store();
                $('#changesetting').css("display", "block");

            } else {
                $('#changesetting').css("display", "none");
            }
        } else (
            alert(data.msg)
        )
    }
})

//Choọn Quầy Hàng

$.ajax({
    url: '/home/Stalls',
    type: 'get',
    success: function (data) {
        $('#Stalls').empty();
        if (data.code == 200) {
            let table = '<option value="' + data.id + '">' + data.name + '</option>'
            $.each(data.c, function (k, v) {
                table += '<option value="' + v.id + '">' + v.name + '</option>'
            });
            $('#Stalls').append(table);
        } else (
            alert(data.msg)
        )
    }
})
$('#Store').on('change', function () {
    var idstore = $("#Store option:selected").val();
    Stalls(idstore)
})

//END Choọn Quầy Hàng

//Select::...

function WareHouse() {
    $.ajax({
        url: '/home/WareHouse',
        type: 'get',
        success: function (data) {
            $('#WareHouses').empty();
            if (data.code == 200) {
                let table = '<option value="' + data.id + '">' + data.name + '</option>'
                $.each(data.c, function (k, v) {
                    table += '<option value="' + v.id + '">' + v.name + '</option>'
                });
                $('#WareHouses').append(table);
            } else {
                alert(data.msg)
            }
        }
    })
}
function Store() {
    $.ajax({
        url: '/home/Store',
        type: 'get',
        success: function (data) {
            $('#Store').empty();
            if (data.code == 200) {
                let table = '<option value="' + data.id + '">' + data.name + '</option>'
                $.each(data.c, function (k, v) {
                    table += '<option value="' + v.id + '">' + v.name + '</option>'
                });
                $('#Store').append(table);
            } else {
                alert(data.msg)
            }
        }
    })
}
function Stalls(idstore) {
    $.ajax({
        url: '/home/Stalls',
        type: 'get',
        data: {
            idstore
        },
        success: function (data) {
            $('#Stalls').empty();
            if (data.code == 200) {
                let table = '<option value="' + data.id + '">' + data.name + '</option>'
                $.each(data.c, function (k, v) {
                    table += '<option value="' + v.id + '">' + v.name + '</option>'
                });
                $('#Stalls').append(table);
            } else {
                alert(data.msg)
            }
        }
    })
}

//END Select::...

// END Chọn Nơi Xuất Hàng Bán

//-------------------------------------------------------------------------------------------------------------------------------------------------------

//Lưu Nơi Xuất Hàng Bán

function Save() {
    var WareHouses = $("#WareHouses option:selected").val();
    var Store = $("#Store option:selected").val();
    var Stalls = $("#Stalls option:selected").val();
    if (WareHouses == -1) {
        alert("Chọn Kho Hàng !!!")
        return;
    } if (Store == -1) {
        alert("Chọn Cửa hàng !!!")
        return;
    } if (Stalls == -1) {
        alert("Chọn Quầy Bán !!!")
        return;
    }
    $.ajax({
        url: '/home/SaveSetting',
        type: 'get',
        data: {
            WareHouses, Store, Stalls
        },
        success: function (data) {
            if (data.code == 200) {
                $('#changesetting').css("display", "none");
                UpLoadft();
            } else if (data.code == 300) {
                $('#changesetting').css("display", "block");
            }
            else {
                alert(data.msg)
            }
        }
    })
}

//END Lưu Nơi Xuất Hàng Bán

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Hiển Thị Thông Tin Nơi Bán Cuối Màn Hình

UpLoadft();

function UpLoadft() {
    $.ajax({
        url: '/home/UpLoadft',
        type: 'get',
        success: function (data) {
            $('div[name="WareHouse"]').empty()
            $('div[name="Store"]').empty()
            $('div[name="Stalls"]').empty()
            $('div[name="Date"]').empty()
            $('div[name="SaleShift"]').empty()
            $('div[name="ThuNgan"]').empty()
            if (data.code == 200) {
                $('div[name="WareHouse"]').append("Kho Hàng : " + data.namewarehouse)
                $('div[name="Store"]').append("Cửa Hàng : " + data.namestore)
                $('div[name="Stalls"]').append("Quầy Bán : " + data.namestalls)
                var d = new Date();
                $('div[name="Date"]').append("Ngày Bán : " + d.getDate() + "/" + (parseInt(d.getMonth()) + 1) + "/" + d.getFullYear())
                var Dem_gio = setInterval(function () {
                    $('div[name="Time"]').empty()
                    var d = new Date();
                    $('div[name="Time"]').append("Giờ bán : " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds())
                }, 1000);
                if (d.getHours() >= 6 && d.getHours() < 14) {
                    $('div[name="SaleShift"]').append("Ca Bán : 1")
                } else if (d.getHours() >= 14 && d.getHours() <= 22) {
                    $('div[name="SaleShift"]').append("Ca Bán : 2")
                } else {
                    $('div[name="SaleShift"]').append("Ca Bán : 3")
                }
                $('div[name="ThuNgan"]').append("Thu Ngân : " + data.iduser)
            }
            else {
                alert(data.msg)
            }
        }
    })
}

//Hiển Thị Thông Tin Nơi Bán Cuối Màn Hình

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Mã Hàng Bán

$('input[name="idgoods"]').keypress(function (e) {
    if (e.which  == 13) {
        var id = $('input[name="idgoods"]').val().trim();
        $.ajax({
            url: '/home/ChangeBarcode',
            type: 'get',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    Goods(data.c);
                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

//END Nhập Mã Hàng Bán

//-----------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Mã Nhân Viên

$('input[name="iduser"]').keypress(function (e) {
    if (e.which == 13) {
        var id = $('input[name="iduser"]').val().trim();
        $.ajax({
            url: '/home/UserTV',
            type: 'get',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    $.each(data.c, function (k, v) {
                        $('input[name="nameuser"]').val(v.name)
                    })

                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

//END Nhập Mã Nhân Viên

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Mã Khách Hàng

$('input[name="idcustomer"]').keypress(function (e) {
    var id = $('input[name="idcustomer"]').val().trim();
    if (e.which == 13) {
        $.ajax({
            url: '/home/Customer',
            type: 'get',
            data: {
                id
            },
            success: function (data) {
                $('h1[name="namecustomer"]').empty()
                $('h1[name="namepoint"]').empty()
                if (data.code == 200) {
                    $.each(data.c, function (k, v) {
                        $('h1[name="namecustomer"]').append(v.name)
                        $('h1[name="namepoint"]').append(v.point)
                    })

                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

//END Nhập Mã Khách Hàng

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Hiển Thị Sản Phẩm Bán Hàng

function Goods(id) {
    $.ajax({
        url: '/receipt/ChangeGood',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    var ids = $('.id').map(function () {
                        return this.id;
                    }).get();
                    if (ids.includes(v.idgood)) {
                        var amounts = $('#HH' + v.idgood + ' #amount' + v.idgood + '').text();
                        var price = $('#HH' + v.idgood + ' #price' + v.idgood + '').text();
                        var discount = $('#HH' + v.idgood + ' #discount' + v.idgood + '').text();
                        $('#HH' + v.idgood + ' #amount' + v.idgood + '').empty();
                        $('#HH' + v.idgood + ' #amount' + v.idgood + '').append(Number(amounts) + 1);
                        ValidateAmount(v.idgood)
                        $('#HH' + v.idgood + ' #totalmoney' + v.idgood + '').empty()
                        var sum = Number(price) * (Number(amounts) + 1);
                        $('#HH' + v.idgood + ' #totalmoney' + v.idgood + '').append(sum + (sum * (Number(discount) / 100)))
                    } else {
                        let table = '<tr name="detailgoods" id="HH' + v.idgood + '" data-epc="">';
                        table += '<td class="id" id="' + v.idgood + '">' + v.idgood+ '</td>'
                        table += '<td>' + v.name + '</td>'
                        table += '<td>' + v.size + '</td>'
                        table += '<td class="amount" id="amount' + v.idgood + '">1</td>'
                        table += '<td class="price" id="price' + v.idgood + '">' + (v.price) + '</td>'
                        table += '<td class="discount" id="discount' + v.idgood + '">' + v.discount + '</td>'
                        table += '<td class="totalmoney" id="totalmoney' + v.idgood + '"></td>'
                        table += '<td>' + v.categoods + '</td></tr>'
                        $('#tbd').append(table);
                        ValidateAmount(v.idgood)
                        var amount = document.getElementById('amount' + v.idgood + '').innerText
                        $('#HH' + v.idgood +' #totalmoney' + v.idgood + '').append(Number(v.price) * Number(amount) + (Number(v.price) * Number(v.discount) / 100))
                        $('input[name="idgoods"]').val('')                      
                    }
                })
                TongGiaTri()

            } else if (data.code == 1) {

            }
               /* alert(data.msg)*/
        }
    })
}

//Hiển Thị Sản Phẩm Bán Hàng

//------------------------------------------------------------------------------------------------------------------------------------------------------

//kiem tra hang ton

function ValidateAmount(id) {
    $.ajax({
        url: '/home/TonKho',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            var amounts = amounts = $('#HH' + id + ' #amount' + id + '').text();
            var price = $('#HH' + id + ' #price' + id + '').text();
            var discount = $('#HH' + id + ' #discount' + id + '').text();
            if (data.code == 200) {                
                    if (data.c < Number(amounts)) {
                        $('#HH' + id + ' #amount' + id + '').text(data.c);
                        $('input[name="amountgoods"]').val(data.c)
                        $('#HH' + id + ' #totalmoney' + id + '').empty()
                        var sum = Number(price) * (data.c);
                        $('#HH' + id + ' #totalmoney' + id + '').append(sum + (sum * (Number(discount) / 100)))
                        alert("Số Lượng Vượt Hàng Tồn !!!")
                        TongGiaTri()
                    }                      
            } else {
                alert(data.msg)
            }

        }
    })
}

//END kiem tra hang ton

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Đổi Đơn Vị Tiền Tệ VND

function Money(money) {
    var formatter = new Intl.NumberFormat('en-vi', {
        style: 'currency',
        currency: 'VND',
    });
    var res = formatter.format(money);
    return res;
}

//END Đổi Đơn Vị Tiền Tệ VND

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Quét Mã Vạch Barcode

$(document).scannerDetection({
    timeBeforeScanTest: 200, // wait for the next character for upto 200ms
    startChar: [120],
    endChar: [13], // be sure the scan is complete if key 13 (enter) is detected
    avgTimeByChar: 40, // it's not a barcode if a character takes longer than 40ms
    ignoreIfFocusOn: 'input', // turn off scanner detection if an input has focus
    minLength: 1,
    onComplete: function (barcode, qty) {
        Goods(barcode)

    }, // main callback function
    scanButtonKeyCode: 116, // the hardware scan button acts as key 116 (F5)
    scanButtonLongPressThreshold: 5, // assume a long press if 5 or more events come in sequence
    onError: function (string) { alert('Error ' + string); }
});

//END Quét Mã Vạch Barcode

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Hiển Thị Chi Tiết Sản Phẩm Bán

$(document).on('click', 'tr[name="detailgoods"]', function (e) {
    var id = $(this).children('.id').attr('id')
    var amount = $(this).children('.amount').text()
    var discount = $(this).children('.discount').text();
    $('input[name="idgoods"]').val(id)
    $('input[name="amountgoods"]').val(amount)
    $('input[name="discountgoods"]').val(discount)
})

//END Hiển Thị Chi Tiết Sản Phẩm Bán

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Số Lượng Sản Phẩm Bán

$('input[name="amountgoods"]').on('keypress', function (e) {
    var id = $('input[name="idgoods"]').val().trim();
    $('#HH' + id + ' #amount' + id + '').empty();
    if (e.which == 13) {
        $('#HH' + id + ' #amount' + id + '').append($('input[name="amountgoods"]').val().trim())
        var price = $('#HH' + id + ' #price' + id + '').text();
        var discount = $('#HH' + id + ' #discount' + id + '').text();
        var amounts = $('#HH' + id + ' #amount' + id + '').text();
        ValidateAmount(id)
        $('#HH' + id + ' #totalmoney' + id + '').empty()
        var sum = Number(price) * (Number(amounts));
        $('#HH' + id + ' #totalmoney' + id + '').append(sum + (sum * (Number(discount) / 100)))
        TongGiaTri()
    }
})

//End Nhập Số Lượng Sản Phẩm Bán

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Chiết Khấu Sản Phẩm Bán

$('input[name="discountgoods"]').on('keypress', function (e) {
    var id = $('input[name="idgoods"]').val().trim();
    $('#HH' + id + ' #discount' + id + '').empty();
    if (e.which == 13) {
        $('#HH' + id + ' #discount' + id + '').append($('input[name="discountgoods"]').val().trim())
        var price = $('#HH' + id + ' #price' + id + '').text();
        var discount = $('#HH' + id + ' #discount' + id + '').text();
        ValidateAmount(id)
        $('#HH' + id + ' #totalmoney' + id + '').empty()
        var sum = Number(price) * (Number($('input[name="amountgoods"]').val().trim()));
        $('#HH' + id + ' #totalmoney' + id + '').append(sum - (sum * (Number(discount) / 100)))
        TongGiaTri()
    }
})

//END Nhập Chiết Khấu Sản Phẩm Bán

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhập Chiết Khấu Hóa Đơn

$('input[name="discount"]').on('keypress', function (e) {
    if (e.which == 13) {
        TongGiaTri();
        if ($('input[name="discount"]').val().trim().length == 0) {
            $('input[name="discount"]').val(0);
            TongGiaTri();
        }
    }
})

//End Nhập Chiết Khấu Hóa Đơn

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Tổng Tiền Khách Phải Trả

function TongGiaTri() {
    var totalmoney = $(".totalmoney").map(function () {
        return $(this).text();
    }).get();
    $('h1[name="price"]').empty();
    var sumtotalmoney = 0;
    for (let i = 0; i < totalmoney.length; i++) {
        sumtotalmoney += parseInt(totalmoney[i])
    }
    $('h1[name="price"]').append(Money(sumtotalmoney))
    $('h1[name="sumprice"]').empty();
    var ck = $('input[name="discount"]').val().trim();
    $('h1[name="sumprice"]').append(Money(sumtotalmoney - (sumtotalmoney* (parseInt(ck)/100))))
}

//END Tổng Tiền Khách Phải Trả

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Nhấn Tính Tiền

$('button[name="SaveBill"]').click(function () {
    $('h1[name="sumprice2"]').empty();
    $('h1[name="sumprice2"]').append($('h1[name="sumprice"]').text())
    var idcustomer = $('input[name="idcustomer"]').val();
    var sumprice = $('h1[name="sumprice2"]').text().substring(1).replace(/,/g, '');
    $.ajax({
        url: '/home/AddBill',
        type: 'post',
        data: {
            idcustomer, sumprice
        },
        success: function (data) {
            if (data.code == 200) {
                var idgoodss = $(".id").map(function () {
                    return $(this).text();
                }).get();
                for (let i = 0; i < idgoodss.length; i++) {
                    var idgoods = idgoodss[i]
                    var amounts = $('#HH' + idgoodss[i] + ' #amount' + idgoodss[i] + '').text();
                    var price = $('#HH' + idgoodss[i] + ' #price' + idgoodss[i] + '').text();
                    var discount = $('#HH' + idgoodss[i] + ' #discount' + idgoodss[i] + '').text();
                    var totalmoney = $('#HH' + idgoodss[i] + ' #totalmoney' + idgoodss[i] + '').text();
                    $.ajax({
                        url: '/home/AddDetailBill',
                        type: 'post',
                        data: {
                            idgoods, amounts, price, discount, totalmoney
                        },
                        success: function (data) {
                            if (data.code == 200) {
                                $('#TinhTien').css("display", "block")
                                DeleteEPC()
                            }
                            else if (data.code == 100) {
                                alert(data.msg)
                            }
                            else if (data.code == 1) {
                                alert(data.msg)
                            } else if (data.code==2) {
                                alert(data.msg)
                            }
                            else {
                                alert(data.msg)
                            }
                        },

                    })                 
                }             
            } else {
                alert(data.msg)
            }
        }
    })
})

//END Nhấn Tính Tiền

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Tính Số Tiền Trả Lại Cho Khách

$('input[name="TienKhachTra"]').keyup(function () {
    $('h1[name="TraLai"]').empty();
    var a = $('input[name="TienKhachTra"]').val().trim();
    var b = $('h1[name="sumprice2"]').text().substring(1).replace(/,/g, '')
    $('h1[name="TraLai"]').append(Money(parseInt(a) - parseInt(b)))
})

//END Tính Số Tiền Trả Lại Cho Khách

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Máy In

const $estado = document.querySelector("#estado"),
    $listaDeImpresoras = document.querySelector("#listaDeImpresoras"),
    $btnLimpiarLog = document.querySelector("#btnLimpiarLog"),
    $btnRefrescarLista = document.querySelector("#btnRefrescarLista"),
    $btnEstablecerImpresora = document.querySelector("#btnEstablecerImpresora"),
    $texto = document.querySelector("#texto"),
    $impresoraSeleccionada = document.querySelector("#impresoraSeleccionada"),
    $btnImprimir = document.querySelector("#btnImprimir");

const loguear = texto => $estado.textContent += (new Date()).toLocaleString() + " " + texto + "\n";
const limpiarLog = () => $estado.textContent = "";

$btnLimpiarLog.addEventListener("click", limpiarLog);

const limpiarLista = () => {
    for (let i = $listaDeImpresoras.options.length; i >= 0; i--) {
        $listaDeImpresoras.remove(i);
    }
};

const obtenerListaDeImpresoras = () => {
    loguear("Getting printers...");
    Impresora.getImpresoras()
        .then(listaDeImpresoras => {
            refrescarNombreDeImpresoraSeleccionada();
            loguear("Printers loaded");
            limpiarLista();
            listaDeImpresoras.forEach(nombreImpresora => {
                const option = document.createElement('option');
                option.value = option.text = nombreImpresora;
                $listaDeImpresoras.appendChild(option);
            })
        });
}

const establecerImpresoraComoPredeterminada = nombreImpresora => {
    loguear("Setting printer...");
    Impresora.setImpresora(nombreImpresora)
        .then(respuesta => {
            refrescarNombreDeImpresoraSeleccionada();
            if (respuesta) {
                loguear(`Printer ${nombreImpresora} set successfully`);
            } else {
                loguear(`Cannot set the printer ${nombreImpresora}`);
            }
        });
};

const refrescarNombreDeImpresoraSeleccionada = () => {
    Impresora.getImpresora()
        .then(nombreImpresora => {
            $impresoraSeleccionada.textContent = nombreImpresora;
        });
}


$btnRefrescarLista.addEventListener("click", obtenerListaDeImpresoras);
$btnEstablecerImpresora.addEventListener("click", () => {
    const indice = $listaDeImpresoras.selectedIndex;
    if (indice === -1) return loguear("No printers")
    const opcionSeleccionada = $listaDeImpresoras.options[indice];
    establecerImpresoraComoPredeterminada(opcionSeleccionada.value);
});

$btnImprimir.addEventListener("click", () => {
    var refunds = $('h1[name="TraLai"]').text().substring(1).replace(/,/g, '');
    var th = $('h1[name="sumprice2"]').text();
    var tkt = $('#TienKhachTra').val().trim();
    var ttl = $('h1[name="TraLai"]').text();
    $.ajax({
        url: '/home/Refunds',
        type: 'post',
        data: {
            refunds, th, tkt, ttl
        },
        success: function (data) {
            if (data.code == 200) {
                IN(th, tkt, ttl)
            }
        }
    })  
});
$('#inlai').click(function() {
    $.ajax({
        url: '/home/InLai',
        type: 'get',
        success: function (data) {
            if (data.code == 300) {
                IN(data.th, data.tkt, data.ttl)
            } else if (data.code == 200) {
                alert("Bạn Không Có Quyền In Lại Hóa Đơn !!!")
            }
        }
    })
});
// En el init, obtenemos la lista
obtenerListaDeImpresoras();

//END Máy In

//------------------------------------------------------------------------------------------------------------------------------------------------------


//In Bill

function IN(th, tkt, ttl) {
    
    $.ajax({
        url: '/home/BILL',
        type: 'get',
        success: function (data) {
            var Stt = 1;
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let impresora = new Impresora();
                    impresora.write("\n");
                    impresora.cut();
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("center");
                    impresora.write(v.store+ "\n");
                    impresora.write(v.address + "\n");
                    impresora.setFontSize(2, 2);
                    impresora.write("*****\n");
                    impresora.write("Hóa Đơn Thanh Toán\n");
                    impresora.write("------------------------\n");
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("left");
                    impresora.write("Số HD : "+v.idbill+"\t");
                    impresora.setAlign("right");
                    impresora.write("Quầy Bán : "+v.stalls + "\n");
                    impresora.setAlign("left");
                    impresora.write("Ngày Bán : "+v.date+"\t");
                    impresora.setAlign("right");
                    impresora.write("Giờ bán : "+v.time + "\n");
                    impresora.setAlign("left");
                    impresora.write("Thu Ngân : "+v.userTN+"\t");
                    impresora.setAlign("right");
                    impresora.write("Ca Bán : "+v.refunds + "\n");
                    impresora.setAlign("center");
                    impresora.write("Khách Hàng : "+v.customer + "\n");
                    impresora.write("Điểm Tích Lũy : " + v.point + "\n");
                    impresora.write("--------------------------------\n");
                    impresora.setAlign("left");
                    impresora.write("Stt  Mã HH  Tên Hàng Hóa  SL  CK  Giá  Tổng\n");
                    var a = [];
                    $.each(data.d, function (k, v) {
                        if (a.includes(v.idgood)) {
                            
                        } else {
                            a.push(v.idgood)
                            impresora.write((Stt++) + "  " + v.idgood + "  " + v.namegoods.substring(0, 15) + "  " + v.amount + "  " + v.discount + "  " + Money(v.price) + "  " + Money(v.totalmoney) + "\n");
                            impresora.write("----------------------\n");
                        }                                           
                    })
                    impresora.setAlign("right");
                    impresora.write("Tiền Hàng : " + th + "\n");
                    impresora.write("Tiền Phải Thu : " + th + "\n");
                    impresora.write("Tiền Khách Trả : " + Money(tkt) + "\n");
                    impresora.write("Tiền Trả Lại : " + ttl + "\n");
                    impresora.setAlign("left");
                    impresora.write("Bao Gồm Thuế GTGT 10%\n");
                    impresora.setAlign("center");
                    impresora.write("Mở Cửa Mỗi Ngày Từ\n");
                    impresora.write("Cảm Ơn Và Hẹn Gặp Lại\n");
                    impresora.write("Quý Khách Vui Lòng Mang Hóa Dơn khi Đổi Hàng\n");
                    impresora.write("Thời Hạn Đổi Hàng : 5 Ngày Kể Từ khi Mua\n");   
                    impresora.setAlign("center");
                    impresora.qr(v.id, 80);
                    impresora.setAlign("right");
                    impresora.write("Số Seri : " + v.id + "\n");
             
                    impresora.cut();
                    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
                    impresora.cash();
                    impresora.imprimirEnImpresora($listaDeImpresoras.value);
                })
                window.location.href = "/Home/Index"
               
            }
            else {
                alert(data.msg)
            }
        }
    })
}

//END In Bill

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Báo Cáo Đâù Ca

$('#baocaodauca').click(function () {
    $('#firstreport').modal('show')
})

function AddReportFirst() {
    var tiendauca = $('#tiendauca').val().trim().substring(1).replace(/,/g, "");
    var caban = $('#caban option:selected').val()
    $.ajax({
        url: '/home/AddReportShift',
        type: 'post',
        data: {
            tiendauca, caban
        },
        success: function (data) {
            if (data.code == 200) {
                PrintReportFirstShift();
            }
        }
    })
}

function PrintReportFirstShift() {
    $.ajax({
        url: '/home/PrintReportFirstShift',
        type: 'get',
        success: function (data) {
            var Stt = 1;
            if (data.code == 200) {
                $.each(data.b, function (k, v) {
                    let impresora = new Impresora();
                    impresora.write("\n");
                    impresora.cut();
                    impresora.cutPartial();
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("center");
                    impresora.write(v.store + "\n");
                    impresora.write(v.address + "\n");
                    impresora.setFontSize(2, 2);
                    impresora.write("*****\n");
                    impresora.write("Báo Cáo Đầu Ca\n");
                    impresora.write("------------------------\n");
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("left");
                    impresora.write("Quầy : " + v.stall + "\t");
                    impresora.setAlign("right");
                    impresora.write("Ngày : " + v.date + "\n");
                    impresora.setAlign("left");
                    impresora.write("Ca : " + v.shift + "\n");
                    impresora.setAlign("left");
                    impresora.write("Nhân Viên : " + v.nv + "\n");
                    impresora.write("--------------------------------\n");
                    impresora.write("Tổng Tiền \n");
                    impresora.write("Tiền Đầu Ca:" + Money(v.moneyfirst) + " \n");
                    impresora.setAlign("center");
                    impresora.write("--------------------------------\n");
                    impresora.write("Nhân Viên Kí Tên \n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.qr(v.id, 80);
                    impresora.setAlign("right");
                    impresora.write("Số Seri : " + v.id + " \n");   
                    impresora.cut();
                    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
                    impresora.cash();
                    impresora.imprimirEnImpresora($listaDeImpresoras.value);
                })
                window.location.href = "Home/Index"

            }
            else {
                alert(data.msg)
            }
        }
    })
}

//END Báo Cáo Đâù Ca

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Báo Cáo Kết Ca

$('#baocaoketca').click(function () {
    $('#reportlendshift').modal('show')
})

$('#seri').keypress(function (e) {
    if (e.which == 13) {
        var seri = $('#seri').val().trim();
        First(seri)
        ReportBill(seri)
    }
})

function First(seri) {
    $.ajax({
        url: '/home/First',
        type: 'get',
        data: {
            seri
        },
        success: function (data) {
            if (data.code == 200) {
                $.each(data.b, function (k, v) {
                    $('div[name="nhanvienfirst"]').append(v.nv)
                    $('div[name="Datefirst"]').append(v.date)
                    $('div[name="caban"]').append(v.shift)
                    $('div[name="tiendauca"]').append(Money(v.moneyfirst))
                })
            }
        }
    })
}

function ReportBill(seri) {
    $.ajax({
        url: '/home/ReportBill',
        type: 'get',
        data: {
            seri
        },
        success: function (data) {
            $('#tbdreportbill').empty();
            var Stt = 1;
            if (data.code == 200) {
                $.each(data.b, function (k, v) {
                    let tbdreportbill = '<tr>';
                    tbdreportbill += '<td>' + (Stt++) + '</td>'
                    tbdreportbill += '<td>' + v.id + '</td>'
                    tbdreportbill += '<td name="sumprice">' + v.sumprice + '</td></tr>';
                    $('#tbdreportbill').append(tbdreportbill)
                })
                TienThucBan()
            }
        }
    })
}

function TienThucBan() {
    var sumprice = document.getElementsByName('sumprice');
    var tienthucban = 0;
    for (let i = 1; i < sumprice.length; i++) {
        tienthucban += Number(sumprice[i].innerText)
    }
    $('#tienthucban').val(tienthucban)
}

function AddReportEndShift() {
    var seri = $('#seri').val().trim();
    var tiencuoica = $('#tiencuoica').val().trim();
    var tienthucban = $('#tienthucban').val().trim();
    if (tiencuoica.length == 0) {
        alert("Nhập Tiền Cuối Ca !!")
        return;
    }
    if (tienthucban.length == 0) {
        alert("Nhập Tiền Thực Cuối Ca !!")
        return;
    }
    $.ajax({
        url: '/home/AddReportEndShift',
        type: 'post',
        data: {
            seri,tiencuoica, tienthucban
        },
        success: function (data) {
            if (data.code == 200) {
                PrintReportEndShift();
            }
        }
    })
}

function PrintReportEndShift() {
    $.ajax({
        url: '/home/PrintReportEndShift',
        type: 'get',
        success: function (data) {
            var Stt = 1;
            if (data.code == 200) {
                $.each(data.b, function (k, v) {
                    let impresora = new Impresora();
                    impresora.write("\n");
                    impresora.cut();
                    impresora.cutPartial();
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("center");
                    impresora.write(v.store + "\n");
                    impresora.write(v.address + "\n");
                    impresora.setFontSize(2, 2);
                    impresora.write("*****\n");
                    impresora.write("Báo Cáo Kết Ca\n");
                    impresora.write("------------------------\n");
                    impresora.setFontSize(1, 1);
                    impresora.setAlign("left");
                    impresora.write("Quầy : " + v.stall + "-\t");
                    impresora.setAlign("right");
                    impresora.write("Ngày : " + v.date + "\n");
                    impresora.setAlign("left");
                    impresora.write("Ca : " + v.shift + "\n");
                    impresora.setAlign("left");
                    impresora.write("Nhân Viên : " + v.nv + "\n");
                    impresora.setAlign("center");
                    impresora.write("--------------------------------\n");
                    impresora.write("Số HĐ\tTổng Tiền\n");
                    $.each(data.d, function (k, v) {
                        impresora.write("" + v.id + "\t" + Money(v.sumprice)  + "\n");
                    })
                    impresora.write("--------------------------------\n");
                    impresora.setAlign("left");
                    impresora.write("Tổng Tiền \n");
                    impresora.write("Tiền Đầu Ca:" + Money(v.moneyfirst) + " \n");
                    impresora.write("Tiền Cuối Ca:" + Money(v.moneyend) + " \n");
                    impresora.write("Tiền Thực Bán:" + Money(v.realmoney) + " \n");
                    impresora.setAlign("center");
                    impresora.write("--------------------------------\n");
                    impresora.write("Nhân Viên Kí Tên \n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.write("\n");
                    impresora.qr(v.id, 80);
                    impresora.setAlign("right");
                    impresora.write("Số Seri : " + v.id + " \n");
                    impresora.cut();
                    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
                    impresora.cash();
                    impresora.imprimirEnImpresora($listaDeImpresoras.value);
                })
                window.location.href = "/Home/Index"

            }
            else {
                alert(data.msg)
            }
        }
    })
}

//END Báo Cáo Kết Ca

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Treo Hóa Đơn

var seachhoadontreo = "";

$('#btntreohoadon').click(function () {
    $.ajax({
        url: '/authorization/UserNV7',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                alert("Bạn Không Có Quyền Treo Hóa Đơn !!!")
            } else if (data.code == 300) {
                $('#namehangbill').modal('show')
                $('#TinhTien').css("display", "none")
            }
            else {
                alert(data.msg)
            }
        }
    })

})

$('#hoadontreo').click(function () {
    $('#modalhoadontreo').modal('show')
})

function HangBill() {
    var deshangbill = $('#deshangbill').val().trim();
    var detailgoods = document.getElementsByName('detailgoods');
    $.ajax({
        url: '/home/HangBill',
        type: 'post',
        data: {
            deshangbill
        },
        success: function (data) {
            if (data.code == 200) {
                for (let i = 0; i < detailgoods.length; i++) {
                    var detailgood = detailgoods[i].id.substring(2);
                    TrueStatusDWH(detailgood)
                    if (i == detailgoods.length - 1) {
                        setTimeout(function () { window.location.href = "/Home/Index" },1000)                    
                    }
                }          
            }
        }
    })
}

AllHangBill(seachhoadontreo);

$('#seachhoadontreo').keyup(function () {
    seachhoadontreo = $('#seachhoadontreo').val().trim();
    AllHangBill(seachhoadontreo)
})

function AllHangBill(seachhoadontreo) {
    $.ajax({
        url: '/home/AllHangBill',
        type: 'get',
        data: {
            seachhoadontreo
        },
        success: function (data) {
            if (data.code == 200) {
                var Stt = 1;
                $('#tbdhoadontreo').empty()
                $.each(data.a, function (k, v) {
                    let tbd = '<tr id="' + v.id + '">';
                    tbd += '<td>' + (Stt++) + '</td>';
                    tbd += '<td>' + v.id + '</td>';
                    tbd += '<td>' + v.des + '</td>';
                    tbd += '<td class="action" nowrap="nowrap">';
                    tbd += '<div class="dropdown dropdown-inline">';
                    tbd += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    tbd += '<i class="la la-cog"></i></a>';
                    tbd += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    tbd += '<ul class="nav nav-hoverable flex-column">';
                    tbd += '<li class="nav-item"><a class="nav-link"name="open" onclick="DetailHangBill(' + v.id + ')">';
                    tbd += '<i class="nav-icon la la-print"></i><span class="nav-text">Mở</span></a></li>';
                    tbd += '<li class="nav-item"><a class="nav-link" name="delete" onclick="DeleteHangBill(' + v.id + ')">';
                    tbd += '<i class="nav-icon la la-trash"></i><span class="nav-text">Delete</span></a></li>';
                    tbd += '</ul></div></div>';
                    tbd += '</td>';
                    tbd += '</tr > ';
                    $('#tbdhoadontreo').append(tbd)
                })
            }
        }
    })
}

function DetailHangBill(id) {
    $.ajax({
        url: '/home/DetailHangBill',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                $('#tbd').empty();
                AllHangBill(seachhoadontreo);
                $('#modalhoadontreo').modal('hide')
                $.each(data.a, function (k, v) {                 
                    Goods(v.idgoods)
                })
                DeleteHangBill(id)
            }
        }
    })
}

function DeleteHangBill(id) {
    $.ajax({
        url: '/home/DeleteHangBill',
        type: 'post',
        data: {
            id
        },
        success: function (data) {
            if (data.code == 200) {
                AllHangBill(seachhoadontreo);
            }
        }
    })
}

//END Treo Hóa Đơn

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Mở Ứng Dụng In

function OpenImpresora() {
    $.ajax({
        url: '/home/OpenImpresora',
        type: 'get',
        success: function (data) {

        }
    })

}

//END Mở Ứng Dụng In

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Xóa BILL

function DeleteBill() {
    $.ajax({
        url: '/home/DeleteBill',
        type: 'post',
        success: function (data) {
            if (data.code == 200) {
                window.location.href = "/Home/Index"
            }
        }
    })
}

//END Xóa BILL

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Đóng Bill

function Dong() {
    var detailgoods = document.getElementsByName('detailgoods');
    $.ajax({
        url: '/home/DeleteBill',
        type: 'post',
        success: function (data) {
            if (data.code == 200) {
                for (let i = 0; i < detailgoods.length; i++) {
                    var detailgood = detailgoods[i].id.substring(2);
                    TrueStatusDWH(detailgood)
                }
                $('#TinhTien').css("display", "none")
            }
        }
    })
}

function TrueStatusDWH(detailgood) {
    $.ajax({
        url: '/home/TrueStatusDWH',
        type: 'post',
        data: {
            detailgood
        },
        success: function (data) {
            if (data.code == 200) {
                $('#TinhTien').css("display", "none")
            }
        }
    })
}

//END Đóng Bill

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Xóa Hàng HÓa Bán

$(document).on('click', 'div[name="deleteHH"]', function (e) {
    $.ajax({
        url: '/authorization/UserNV6',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                alert("Bạn Không Có Quyền Xóa !!!")
            } else if (data.code == 300) {
                var id = $('input[name="idgoods"]').val().trim();
                DeleteHH("HH" + id);
                TongGiaTri()}
            else {
                alert(data.msg)
            }
        }
    })
   
})

function DeleteHH(id) {
    document.getElementById(id).remove(); 
}

//END Xóa Hàng HÓa Bán

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Xuất Hàng Hóa Bằng RFID

    setInterval(function () { AllShowEPC() }, 200);

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
                    Goods(v.idgood)
                })

            }
        }
    })
}


//function DeleteEPC() {
//    $.ajax({
//        url: '/rfid/DeleteEPC',
//        type: 'post',
//        success: function (data) {
//            if (data.code == 200) {
//            }
//        }
//    })
//}

//END Xuất Hàng Hóa Bằng RFID

//------------------------------------------------------------------------------------------------------------------------------------------------------

//Thu Gọn, Mở Rộng Phần Tính Tiền

$('.ThuGon').attr("hidden", true)

$('#btnDayDu').click(function () {
    $('.ThuGon').attr("hidden", false)
    $('#btnDayDu').css("display", "none")
    $('#btnRutGon').css("display", "block")
})

$('#btnRutGon').click(function () {
    $('.ThuGon').attr("hidden", true)
    $('#btnDayDu').css("display", "block")
    $('#btnRutGon').css("display", "none")
})

//END Thu Gọn, Mở Rộng Phần Tính Tiền
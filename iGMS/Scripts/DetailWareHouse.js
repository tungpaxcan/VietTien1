﻿var d = new Date()
$('select[name="idReceipt"]').change(function () {
    var idReceipt = $('select[name = "idReceipt"] option:selected').val();
    
    $.ajax({
        url: '/detailwarehouse/Show',
        data: {
            idReceipt
        },
        success: function (data) {
            $('span[name="method"]').empty();
            $('span[name="datepaydh"]').empty();
            $('span[name="namesupplier"]').empty();
            $('span[name="date"]').empty();
            $('span[name="warehouse"]').empty();
            $('tbody[name="tbd"]').empty();
            $('tbody[name="tbd1"]').empty();
            $('#tbdct').empty();
            var Stt = 1;
            var pricetax = 0;
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    $('span[name="method"]').append(v.method)
                    $('span[name="datepaydh"]').append(v.datepaydh)
                    $('span[name="namesupplier"]').append(v.namesupplier)
                    $('span[name="namesupplier"]').attr('id',v.idsupplier)
                    $('span[name="date"]').append(d.getDay() + "/" + d.getMonth() + "/" + d.getFullYear())
                    $('span[name="warehouse"]').append(v.warehouse)
                    $('span[name="Tong"]').append(to_vietnamese(v.sumprice))
                    $('span[name="warehouse"]').attr('id', v.idwarehouse)
                })
                $.each(data.d, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td name="idgoods" data-amount="' + v.amount + '">' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td>' + v.amount + '</td>'
                    table += '<td>' + v.price + '</td>'
                    table += '<td>' + v.discount + '</td>'
                    table += '<td>' + v.pricediscount + '</td>'
                    table += '<td>' + v.tax + '</td>'
                    table += '<td>' + v.pricetax + '</td>'
                    table += '<td>' + v.sumprice + '</td>'

                    table += '</tr>';
                    pricetax += Number(v.pricetax)
                    $('tbody[name="tbd"]').append(table);
                    let table1 = '<tr id="' + v.id + '" role="row" class="odd">';
                    table1 += '<td>' + (Stt++) + '</td>'
                    table1 += '<td>' + v.id + '</td>'
                    table1 += '<td>' + v.name + '</td>'
                    table1 += '<td>' + v.unit + '</td>'
                    table1 += '<td>' + v.amount + '</td>'
                    table1 += '<td>' + v.price + '</td>'
                    table1 += '<td>' + v.discount + '</td>'
                    table1 += '<td>' + v.pricediscount + '</td>'
                    table1 += '<td>' + v.tax + '</td>'
                    table1 += '<td>' + v.pricetax + '</td>'
                    table1 += '<td>' + v.sumprice + '</td>'

                    table += '</tr>';
                    pricetax += Number(v.pricetax)
                    $('tbody[name="tbd1"]').append(table1);

                });
                $.each(data.c, function (k, v) {
                    let table = '<tr role="row" class="odd">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + idReceipt + '</td>'
                    table += '<td>' + idReceipt + '</td>'
                    table += '<td>' + d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + '</td>'
                    table += '<td id="sumpricetax">' + pricetax + '</td>'
                    table += '<td id="sumpriceli">' + v.sumprice + '</td>'

                    table += '</tr>';
                    $('#tbdct').append(table);
                });
            }
        }
    })
})

//------------------------Add----------------------

function Add() {
    var idReceipt = $('select[name = "idReceipt"] option:selected').val();
    var idwarehouse = $('span[name="warehouse"]').attr('id');
    var idsupplier = $('span[name="namesupplier"]').attr('id')
    var idgoods = document.getElementsByName('idgoods')
    for (let i = 0; i < idgoods.length; i++) {
        var idgood = idgoods[i].innerText;
        var amount = idgoods[i].getAttribute("data-amount");
        $.ajax({
            url: '/detailwarehouse/Add',
            type: 'post',
            data: {
                idwarehouse, idsupplier, idgood, amount, idReceipt
            },
            success: function (data) {
                $('#BILL').modal('show')
                Last()
            }
        })
    }
 
}
function AddCT() {
    var id = $('select[name="idReceipt"]').val().trim();
    var sumpricetax = $('#sumpricetax').text()
    var sumpriceli = $('#sumpriceli').text()
    $.ajax({
        url: '/detailWareHouse/AddCT',
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
                window.location.href = "/DetailWareHouse/Index"
            } else if (data.code == 300) {
                alert(data.msg)
            } else {
                alert(data.msg)
            }
        }
    })
}
//-------------Last----------
function Last() {
    $.ajax({
        url: '/detailwarehouse/Last',
        type: 'get',
        success: function (data) {
            $('span[name="idpn"]').empty()
            $('span[name="datepn"]').empty()
            if (data.code == 200) {
                $('span[name="idpn"]').append(data.idpn)
                $('span[name="datepn"]').append(d.getDay() + "/" + d.getMonth() + "/" + d.getFullYear())
            }
        }
    })
}
$('#btnct').click(function () {
    $('#CT').modal('show')
})
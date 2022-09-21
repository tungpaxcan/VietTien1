var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
Goods(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    Goods(pagenum, page, seach);


});


function Goods(pagenum, page, seach) {
    $.ajax({
        url: '/goods/List',
        type: 'get',
        data: { pagenum, page, seach },
        success: function (data) {
            var Stt = 1;
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td>' + (Stt++) + '</td>'
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.idgood + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.unit + '</td>'
                    table += '<td>' + v.price + '</td>'
                    table += '<td>' + v.pricetax + '</td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Goods/Details/' + v.id + '">';
                    table += '<i class="nav-icon icon-xl la la-building"></i>';
                    table += '<span class="nav-text">Detail</span></a></li>';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Goods/Edits/' + v.id + '">';
                    table += '<i class="nav-icon la la-edit"></i>';
                    table += '<span class="nav-text">Edit </span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" onclick="printDiv(\'barcodehh' + v.id + '\')">';
                    table += '<i class="nav-icon la la-print"></i><span class="nav-text">Print</span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" name="delete">';
                    table += '<i class="nav-icon la la-trash"></i><span class="nav-text">Delete</span></a></li>';
                    table += '</ul></div></div>';
                    table += '</td>';
                    table += '</tr>';
        
                    $('#tbd').append(table);
                });

                //--------------------------------
                let kt_datatable_info = 'Showing 1 to ' + pagenum + ' of ' + data.count + ' entries'
                $('#kt_datatable_info').append(kt_datatable_info);
                //-----------------------------page---------------------------
                $('#page').empty();
                if (parseInt(page) >= 2) {
                    let pagemin = '<li id="' + 1 + '"><a class="a_1 a_2" >' + 1 + '...</a></li>';
                    $('#page').append(pagemin);
                    let pre = ' <li id="' + (parseInt(page) - 1) + '" class="paginate_button page-item previous disabled" >';
                    pre += '<a  aria-controls="kt_datatable" data-dt-idx="0" tabindex="0" class="page-link">';
                    pre += '<i class="ki ki-arrow-back"></i></a></li>';
                    $('#page').append(pre);
                }
                for (let i = parseInt(page); i <= (parseInt(page) + 4); i++) {
                    if (i == data.pages + 1) {
                        return;
                    }
                    let li = '<li id="' + i + '" class="paginate_button page-item ">';
                    li += '<a aria-controls="kt_datatable" data-dt-idx="1" tabindex="0" class="page-link">' + i + '</a></li>';
                    $('#page').append(li);


                }


                let next = '<li  id="' + (parseInt(page) + 1) + '" class="paginate_button page-item next" id="kt_datatable_next">';
                next += '<a href="#" aria-controls="kt_datatable" data-dt-idx="6" tabindex="0" class="page-link"><i class="ki ki-arrow-next"></i></a></li>';
                $('#page').append(next);

                let pagemax = '<li id="' + data.pages + '"><a class="a_1 a_2" >...' + data.pages + '</a></li>';
                $('#page').append(pagemax);

            } else (
                alert(data.msg)
            )
        }
    })
}

$('#pagenum').on('change', function () {
    var pagenum = $("#pagenum option:selected").val();
    var page = 1;
    var seach = "";
    Goods(pagenum, page, seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
    page = 1;
    seach = $('#seach').val();
    Goods(pagenum, page, seach);
});



//----------------Add::Goods---------------------
function Add() {
    var id = $('#id').val().trim();
    var idgood = $('#idgood').val().trim();
    var style = $("#style option:selected").val();
    var color = $("#color option:selected").val();
    var size = $("#size option:selected").val();
    var name = $("#name").val().trim();
    var gender = $("#gender option:selected").val();
    var categoods = $("#categoods option:selected").val();
    var groupgoods = $("#groupgoods option:selected").val();
    var price = $("#price").val().trim();
    var season = $("#season option:selected").val();
    var coo = $("#coo option:selected").val();
    var material = $("#material").val().trim();
    var company = $("#company").val().trim()
    var pricenew = $("#pricenew").val().trim();
    if ($('#supplier').val().trim().length <= 0) {
        alert("Chọn Nhà Cung Cấp !!!")
        return;
    }
    var tags = JSON.parse($('#supplier').val());
    var TagArray = [];
    //Convert to array
    for (let i = 0; i < tags.length; i++) {
        TagArray.push(tags[i].value.substring(0, tags[i].value.indexOf(' :')))
    }
    if (id.length <= 0) {
        alert("Nhập barcode !!!")
        return;
    } if (style == -1) {
        alert("Chọn Phong Cách !!!")
        return;
    } if (color == -1) {
        alert("Chọn Màu Sắc !!!")
        return;
    } if (size == -1) {
        alert("Chọn Kích Thước !!!")
        return;
    }if (name.length <= 0) {
        alert("Nhập Tên Hàng Hóa")
        return;
    } if (gender == -1) {
        alert("Chọn Giới Tính !!!")
        return;
    }if (categoods == -1) {
        alert("Chọn Loại hàng !!!")
        return;
    } if (groupgoods == -1) {
        alert("Chọn Nhóm hàng!!!")
        return;
    } if (price.length <= 0) {
        alert("Chọn Giá!!!")
        return;
    } if (season == -1) {
        alert("Chọn Mùa!!!")
        return;
    } if (coo == -1) {
        alert("Chọn Nơi Sản Xuất!!!")
        return;
    } if (material <= 0) {
        alert("Nhập Thành Phần !!!")
        return;
    } if (company <= 0) {
        alert("Nhập Công Ty !!!")
        return;
    } if (pricenew <= 0) {
        alert("Nhập Giá Mới !!!")
        return;
    }
    $.ajax({
        url: '/goods/Add',
        type: 'post',
        data: {
            id, idgood, style, color, size, name, gender, categoods
            , groupgoods, price, season, coo, material, company, pricenew

        },
        success: function (data) {
            if (data.code == 200) {
                for (let i = 0; i < TagArray.length; i++) {
                    var supplier = TagArray[i]
                    $.ajax({
                        url: '/suppliergoods/Add',
                        type: 'post',
                        data: {
                            id, supplier
                        },
                        success: function (data) {
                            if (data.code == 200) {
                                if (i == TagArray.length - 1) {
                                    Swal.fire({
                                        title: "Tạo Hàng Hóa Thành Công",
                                        icon: "success",
                                        buttonsStyling: false,
                                        confirmButtonText: "Confirm me!",
                                        customClass: {
                                            confirmButton: "btn btn-primary"
                                        }
                                    });
                                    window.location.href = "/Goods/Index";
                                }


                            } else if (data.code == 300) {
                                alert(data.msg)
                            }
                            else {
                                alert("Tạo  Thất Bại")
                            }
                        },
                    })
                }
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Tạo Hàng Hóa Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}

//----------------Edit::Goods---------------------
function Edit() {
    var id = $('#id').val().trim();
    var categoods = $("#categoods option:selected").val();
    var name = $("#name").val().trim();
    var price = $("#price").val().trim()
    var pricetax = $("#pricetax").val().trim();
    var internalprice = $("#internalprice").val().trim()
    var gtgtinternaltax = $("#gtgtinternaltax").val().trim();
    var discount = $("#discount").val().trim();
    var internaldiscount = $("#internaldiscount").val().trim();
    var expiry = $("#expiry").val().trim().length == 0 ? "01/01/9991" : $("#expiry").val().trim()
    var warrantyperiod = $("#warrantyperiod").val().trim().length == 0 ? "01/01/9991" : $("#warrantyperiod").val().trim()
    var minimuminventory = $("#minimuminventory").val().trim().length == 0 ? 0 : $("#minimuminventory").val().trim()
    var maximuminventory = $("#maximuminventory").val().trim().length == 0 ? 1e10 : $("#maximuminventory").val().trim()
    var des = $("#des").val().trim().length == 0 ? "Không Có" : $("#des").val().trim();;
    var unit = $("#unit1 option:selected").val();
    var season = $("#season option:selected").val();
    var color = $("#color option:selected").val();
    var size = $("#size option:selected").val();
    if ($('#supplier').val().trim().length <= 0) {
        alert("Chọn Nhà Cung Cấp !!!")
        return;
    }
    var tags = JSON.parse($('#supplier').val());
    var TagArray = [];
    //Convert to array
    for (let i = 0; i < tags.length; i++) {
        TagArray.push(tags[i].value.substring(0, tags[i].value.indexOf(' :')))
    }
    if (name.length <= 0) {
        alert("Nhập Tên Hàng Hóa")
        return;
    } if (categoods == -1) {
        alert("Chọn Loại hàng !!!")
        return;
    } if (unit == -1) {
        alert("Chọn Đơn Vị !!!")
        return;
    } if (/[a-zA-Z]/.test(expiry)) {
        alert("Sai Hạn Sử Dụng !!!")
        return;
    } if (/[a-zA-Z]/.test(warrantyperiod)) {
        alert("Sai Ngày Bảo Hành !!!")
        return;
    }

    $.ajax({
        url: '/goods/Edit',
        type: 'post',
        data: {
            id, categoods, name, price, pricetax,
            internalprice, gtgtinternaltax, discount, internaldiscount,
            expiry, warrantyperiod, minimuminventory, maximuminventory, des, unit,
            season, color, size

        },
        success: function (data) {
            if (data.code == 200) {
                    $.ajax({
                        url: '/suppliergoods/Delete',
                        type: 'post',
                        data: {
                            id
                        },
                        success: function (data) {
                            if (data.code == 200) {
                                    for (let j = 0; j < TagArray.length; j++) {
                                        var supplier = TagArray[j]
                                        $.ajax({
                                            url: '/suppliergoods/Add',
                                            type: 'post',
                                            data: {
                                                id, supplier
                                            },
                                            success: function (data) {
                                                if (data.code == 200) {
                                                    if (j == TagArray.length - 1) {
                                                        Swal.fire({
                                                            title: "Sửa Hàng Hóa Thành Công",
                                                            icon: "success",
                                                            buttonsStyling: false,
                                                            confirmButtonText: "Confirm me!",
                                                            customClass: {
                                                                confirmButton: "btn btn-primary"
                                                            }
                                                        });
                                                        window.location.href = "/Goods/Index";
                                                    }
                                                } else if (data.code == 300) {
                                                    alert(data.msg)
                                                }
                                                else {
                                                    alert("Tạo  Thất Bại")
                                                }
                                            },
                                        })
                                    }
                            } else if (data.code == 300) {
                                alert(data.msg)
                            }
                            else {
                                alert("Sửa  Thất Bại")
                            }
                        },
                    })
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Sửa Hàng Hóa Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}


//----------------Delete::Goods---------------------
$(document).on('click', "a[name='delete']", function () {
    var id = $(this).closest('tr').attr('id');
    if (confirm("Bạn Muốn Xóa Dữ Liệu Này ???")) {
        $.ajax({
            url: '/suppliergoods/Delete',
            type: 'post',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    $.ajax({
                        url: '/goods/Delete',
                        type: 'post',
                        data: {
                            id
                        },
                        success: function (data) {
                            if (data.code == 200) {

                                Swal.fire({
                                    title: "Xóa Hàng Hóa Thành Công",
                                    icon: "success",
                                    buttonsStyling: false,
                                    confirmButtonText: "Confirm me!",
                                    customClass: {
                                        confirmButton: "btn btn-primary"
                                    }
                                });
                                window.location.href = "/Goods/Index";
                            }
                            else {
                                alert(data.msg)
                            }
                        }
                    })
                } else if (data.code == 300) {
                    alert(data.msg)
                }
            },
        })
     
    }
})
//-------------barcode-------------
$('#id').keyup(function () {
    var id = $('#id').val().trim();
    if (id.length == 2) {
        JsBarcode("#barcode", id, {
            format: "EAN2",
        });
    } else if (id.length == 5) {
        JsBarcode("#barcode", id, {
            format: "EAN5",
        });
    } else if (id.length == 7 || id.length==8) {
        JsBarcode("#barcode", id, {
            format: "EAN8",
        });
    }else if (id.length == 11) {
        JsBarcode("#barcode", id, {
            format: "UPC",
        });
    } else if ( id.length==13) {
        JsBarcode("#barcode", id, {
            format: "EAN13",
        });
    } else {
        JsBarcode("#barcode", id, {
            format: "CODE128",
        });
    }
    

})


//---------------idgood----------------
$('#style').change(function () {
    IdGood()
})
$('#color').change(function () {
    IdGood()
})
$('#size').change(function () {
    IdGood()
})

function IdGood() {
    var style = $("#style option:selected").val();
    var color = $("#color option:selected").val();
    var size = $("#size option:selected").val();
    $('#idgood').val(style + "-" + color + "-" + size)
}

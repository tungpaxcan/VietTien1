var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
Store(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    Store(pagenum, page, seach);


});


function Store(pagenum, page, seach) {
    $.ajax({
        url: '/store/List',
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
                    table += '<td>' + v.name + '</td>'
                    table += '<td>' + v.address + '</td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Stalls/Index/' + v.id + '">';
                    table += '<i class="icon-xl la la-receipt"></i>';
                    table += '<span class="nav-text">Quầy Hàng </span></a></li>';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Store/Edits/' + v.id + '">';
                    table += '<i class="nav-icon la la-edit"></i>';
                    table += '<span class="nav-text">Edit </span></a></li>';
                    table += '<li class="nav-item"><a class="nav-link" onclick="printDiv(\'' + v.id + '\')">';
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
    Store(pagenum, page, seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
    page = 1;
    seach = $('#seach').val();
    Store(pagenum, page, seach);
});

//----------------Add::Store---------------------
function Add() {
    var name = $('#name').val().trim();
    var id ="CH"+ $('#id').val().trim();
    var province = $("#province option:selected").text();
    var district = $("#district option:selected").text();
    var town = $("#town option:selected").text();
    var addresss = $("#address").val().trim();
    var address = province + " ," + district + " ," + town + " ," + addresss;
    $('.Loading').css("display", "block");
    if (name.length <= 0) {
        alert("Nhập Tên")
        return;
    } if (province == -1) {
        alert("Chọn Tỉnh Thành")
        return;
    } if (district == -1) {
        alert("Chọn Quận Huyện !!!")
        return;
    } if (town == -1) {
        alert("Chọn Phường Xã !!!")
        return;
    } if (addresss.length <=0) {
        alert("Nhập Địa Chỉ !!!")
        return;
    }

    $.ajax({
        url: '/store/Add',
        type: 'post',
        data: {
            id, name, address
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Tạo Cửa Hàng Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                window.location.href = "/Store/Index";
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Tạo Cửa hàng Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}
//----------------Edit::Store---------------------
function Edit() {
    var name = $('#name').val().trim();
    var id = $('#id').val().trim();
    var address = $("#address").val().trim();
    $('.Loading').css("display", "block");
    if (name.length <= 0) {
        alert("Nhập Tên Cửa Hàng")
        return;
    } if (address.length <= 0) {
        alert("Nhập Địa Chỉ !!!")
        return;
    }

    $.ajax({
        url: '/store/Edit',
        type: 'post',
        data: {
            id, name, address
        },
        success: function (data) {
            if (data.code == 200) {
                Swal.fire({
                    title: "Sửa Cửa Hàng Thành Công",
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Confirm me!",
                    customClass: {
                        confirmButton: "btn btn-primary"
                    }
                });
                window.location.href = "/Store/Index";
            } else if (data.code == 300) {
                alert(data.msg)
            }
            else {
                alert("Sửa Cửa hàng Thất Bại")
            }
        },
        complete: function () {
            $('.Loading').css("display", "none");//Request is complete so hide spinner
        }
    })
}
//----------------Delete::Store---------------------
$(document).on('click', "a[name='delete']", function () {
    var id = $(this).closest('tr').attr('id');
    if (confirm("Bạn Muốn Xóa Dữ Liệu Này ???")) {
        $.ajax({
            url: '/store/Delete',
            type: 'post',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    Swal.fire({
                        title: "Xóa Cửa Hàng Thành Công",
                        icon: "success",
                        buttonsStyling: false,
                        confirmButtonText: "Confirm me!",
                        customClass: {
                            confirmButton: "btn btn-primary"
                        }
                    });
                    window.location.href = "/Store/Index";
                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

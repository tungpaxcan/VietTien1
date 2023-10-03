var pagenum = $("#pagenum option:selected").val();
var page = 1;
var seach = "";
Color(pagenum, page, seach);

//phan trang
$('#page').on('click', 'li', function (e) {
    e.preventDefault();
    page = $(this).attr('id');
    Color(pagenum, page, seach);
});


function Color(pagenum, page, seach) {
    $.ajax({
        url: '/color/List',
        type: 'get',
        data: { pagenum, page, seach },
        success: function (data) {
            $('#tbd').empty();
            $('#kt_datatable_info').empty();
            if (data.code == 200) {
                $.each(data.c, function (k, v) {
                    let table = '<tr id="' + v.id + '" role="row" class="odd">';
                    table += '<td>' + v.id + '</td>'
                    table += '<td>' + v.name + '</td>'
                    table += '<td style="background-color:'+v.color+'"></td>'
                    table += '<td class="action" nowrap="nowrap">';
                    table += '<div class="dropdown dropdown-inline">';
                    table += '<a href="javascript:;" class="btn btn-sm btn-clean btn-icon" data-toggle="dropdown">';
                    table += '<i class="la la-cog"></i></a>';
                    table += '<div class="dropdown-menu dropdown-menu-sm dropdown-menu-right">';
                    table += '<ul class="nav nav-hoverable flex-column">';
                    table += ' <li class="nav-item">';
                    table += '<a class="nav-link" href="/Color/Edits/' + v.id + '">';
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
    Color(pagenum, page, seach)
})


//------------------------tim kiem------------------

$('#seach').on('keyup', function (e) {
    page = 1;
    seach = $('#seach').val();
    Color(pagenum, page, seach);
});

//----------------Add::color---------------------
function Add() {
    var id = $('#id').val().trim();
    var name = $('#name').val().trim();
    let color = $('#color').val();
    let des = $('#des').val().trim();
    let status = $('#status').is(':checked')
    if (name.length <= 0) {alert("Chưa Nhập Tên"); $('#name').css('border-color', "red");return;
    }else {$('#name').css('border-color', "green")}
    if (id.length <= 0) {alert("Chưa Nhập Mã"); $('#id').css('border-color', "red"); return;
    } else { $('#id').css('border-color', "green") }
    $.ajax({
        url: '/color/Add',
        type: 'post',
        data: {
            name, id, color, des,status
        },
        success: function (data) {
            if (data.code == 200) {
                successSwal(data.msg)
                window.location.href = "/Color/Index";
            }
            else {
                alert(data.msg)
            }
        },
    })
}

//----------------Edit::color---------------------
function Edit() {
    var name = $('#name').val().trim()
    var id = $('#id').val().trim();
    let color = $('#color').val();
    let des = $('#des').val().trim();
    let status = $('#status').is(':checked')
    if (name.length <= 0) {alert("Chưa Nhập Tên"); $('#name').css('border-color', "red"); return;
    }else { $('#name').css('border-color', "green") }
    if (id.length <= 0) {alert("Chưa Nhập Mã"); $('#id').css('border-color', "red"); return;
    }else { $('#id').css('border-color', "green") }
    $.ajax({
        url: '/color/Edit',
        type: 'post',
        data: {
            id, name, color, des, status
        },
        success: function (data) {
            if (data.code == 200) {
                successSwal(data.msg)
                window.location.href = "/Color/Index";
            }
            else {
                alert(data.msg)
            }
        },
    })
}

//----------------Delete::color---------------------
$(document).on('click', "a[name='delete']", function () {
    var id = $(this).closest('tr').attr('id');
    if (confirm("Xóa " + id + " Này Sẽ Xóa Hết Dữ Liệu Liên Quan Đến " + id + "")) {
        $.ajax({
            url: '/color/Delete',
            type: 'post',
            data: {
                id
            },
            success: function (data) {
                if (data.code == 200) {
                    successSwal(data.msg)
                    Color(pagenum, page, seach);
                }
                else {
                    alert(data.msg)
                }
            }
        })
    }
})

Count_Import()

Name();

//Hiển Thị Tên và id nhân viên đang đăng nhập

function Name() {
    $.ajax({
        url: '/home/UserSession',
        type: 'get',
        success: function (data) {
            $('#namesession').empty();
            $('#idsession').empty();
            if (data.code == 200) {
                $('#namesession').append(data.name);
                $('#idsession').append(data.id);
            } else (
                alert(data.msg)
            )
        }
    })
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//Số lượng đơn cần nhập

function Count_Import() {
    $.ajax({
        url: '/purchaseorder/ListImport',
        type: 'get',
        success: function (data) {
            $('#number-import').empty();
            $('#list-import').empty();
            if (data.code == 200) {
                $.each(data.List_Import, function (k, v) {
                    let list = '<li onclick="Session_Purchase_Order(' + v.id + ')" class="menu-item" aria-haspopup="true">'
                    list += '<a class= "menu-link">'
                    list += '<span class="menu-text">00000' + v.id + '</span>'
                    list += '</a>'
                    list += '</li>'
                    $('#list-import').append(list);
                })
                $('#number-import').append(data.Count_Import);
                Count_Export()
            } else (
                alert(data.msg)
            )
        }
    })
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//Số lượng đơn cần xuất

function Count_Export() {
    $.ajax({
        url: '/purchaseorder/ListExport',
        type: 'get',
        success: function (data) {
            $('#number-export').empty();
            $('#list-export').empty();
            if (data.code == 200) {
                let list =''
                $.each(data.c, function (k, v) {
                    list += '<li onclick="Session_Sales_Order(' + v.id + '0102)" class="menu-item" aria-haspopup="true">'
                    list += '<a class= "menu-link">'
                    list += '<span class="menu-text">00000' + v.id + '</span>'
                    list += '</a>'
                    list += '</li>'
                })
                $.each(data.Sale_Orders, function (k, v) {
                    list += '<li onclick="Session_Sales_Order(' + v.id + ')" class="menu-item" aria-haspopup="true">'
                    list += '<a class= "menu-link">'
                    list += '<span class="menu-text">00000' + v.id + '</span>'
                    list += '</a>'
                    list += '</li>'
                })
                $('#list-export').append(list);
                $('#number-export').append(data.Count_Export);
                Count_Sum()
            } else (
                alert(data.msg)
            )
        }
    })
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//tổng số lượng của nhập và xuất

function Count_Sum() {
    var Count_Import = $('#number-import').text()
    var Count_Export = $('#number-export').text()
    $('#number-sum').append(Number(Count_Import) + Number(Count_Export))
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//lưu Session mã đơn hàng

function Session_Purchase_Order(id) {
    $.ajax({
        url: '/receipt/Session_Id_Purchase',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            window.location.href ="/Receipt/Index"
        }
    })
}

//End

//---------------------------------------------------------------------------------------------------------------------------------------------------

//Lưu Session mã xuất hàng

function Session_Sales_Order(id) {
    $.ajax({
        url: '/receipt/Session_Id_Sales',
        type: 'get',
        data: {
            id
        },
        success: function (data) {
            window.location.href ="/DetailWareHouse/Index2"
        }
    })
}

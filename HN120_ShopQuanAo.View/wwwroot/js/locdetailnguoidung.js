$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    } 
    filterQuantity();

    //$('#minmaxx').show();
    //$('#giaban').hide();
    $('body').on('click', '#add-to-card', function () {
        filterQuantity();
        var sku = '';
        var dongia  ;
       
        var mamau = '';
        var masize = '';
        $('#tb_dataCTSP tbody tr:visible').each(function () {
            if ($(this).find('td').length > 0) {
                sku = $(this).find('td:eq(0)').text().trim();

                dongia = $(this).find('td:eq(5)').text();

                mamau = $(this).find('td:eq(1)').text().trim();
                masize = $(this).find('td:eq(2)').text().trim();
            }
        });
        var magh = $('#maGH_input').val();
        var tensp = $('#tenSP_input').val();

        var sl = $('#quantity_input').val().replace(/\./g, '').split(',')[0];
        var trangthai = 1;
        var dongia_ = parseFloat(dongia);
        var sl_ = parseInt(sl);
        var thanhtien = dongia_ * sl_;
        if (sl_ <=0) {
            return;
        }
        var ghct = {
            MaGioHang: magh,
            SKU: sku,
            TenSp: tensp,
            DonGia: dongia_,
            SoLuong: sl_,
            ThanhTien: thanhtien,
            TrangThai: trangthai
        };

  //      public string MaGioHangChiTiet { get; set; }
		//public string ? MaGioHang { get; set; }
		//public string ? SKU { get; set; }
		//public string ? TenSp { get; set; }
		//public decimal ? DonGia { get; set; }
		//public int ? SoLuong { get; set; }
		//public decimal ? ThanhTien	{ get; set; }
		//public int ? TrangThai { get; set; }
        //$('#giaban').show();
        //
       /* var tensp = $()*/
        // Gửi yêu cầu AJAX tới controller
        $.ajax({
            url: '/CustomerHome/AddToCart', // Thay 'ControllerName' bằng tên controller của bạn
            type: 'POST',
            data: 
                ghct
            ,
            success: function (response) {
                // Xử lý phản hồi từ server nếu cần
                alert('Sản phẩm đã được thêm vào giỏ hàng');
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.error('Đã xảy ra lỗi:', error);
            }
        });
        $.notify('Thêm thành công sản phẩm vào giỏ hàng của bạn', 'error');
    });
    $('#quantity_input').on('change', function () {
        var inputQuantity = parseInt($(this).val());
        var availableQuantity = parseInt($('#total').text());

        if (inputQuantity > availableQuantity) {
            $('#quantity-error').show();
            $(this).val(availableQuantity); // Đặt lại giá trị của input về số lượng tồn
        } else {
            $('#quantity-error').hide();
        }
    });
    let dt = getdulieu();
    console.log(dt);
})
function getdulieu() {
    return {

        SoLuong: $('#quantity_input').val()
    }
}
function filterQuantity() {
    var selectedColor = document.getElementById("group_2").value;
    var selectedSize = document.getElementById("group_1").value;
    var table = document.getElementById("tb_dataCTSP");
   
    var rows = table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    var totalQuantity = 0;

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var color = row.querySelector('[data-mamau]').getAttribute('data-mamau').trim();
        var size = row.querySelector('[data-masize]').getAttribute('data-masize').trim();
        var dg = row.querySelector('[data-madg]').getAttribute('data-madg').trim();
        var dgformat = formatMoney(parseInt(dg));
        var quantity = parseInt(row.querySelector('[data-soluong]').getAttribute('data-soluong').trim());
        if ((selectedColor === '' || color === selectedColor) &&
            (selectedSize === '' || size === selectedSize)) {
            row.style.display = "";
            totalQuantity += quantity;
            $("#minmaxx").html(`<h1>Giá: ${dgformat}</h1>`);
            
        } else {
            row.style.display = "none";
        }
       
    }

    $('#total').text(totalQuantity + ' Sản Phẩm Còn Lại');

}
document.getElementById("actionForm").submit();
function getdataSPCT() {
    
    //$('#tb_dataCTSP tbody tr').each(function () {
    //    if ($(this).find('td').length > 0) {
    //         sku = $(this).find('td:eq(0)').text.trim();
    //    }
    //})
    //return sku;
}
function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}

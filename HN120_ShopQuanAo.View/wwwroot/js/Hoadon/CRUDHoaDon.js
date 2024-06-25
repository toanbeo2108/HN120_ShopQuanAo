$(document).ready(function () {
    $('#bantaiquay').click(function () {
        $('#HoaDonTaiQuayPartialView').show();
    });

    $('#banonline').click(function () {
        $('#HoaDonTaiQuayPartialView').hide();
    });
    $('body').on('click', '#btn_add', function () {
        setdata(null);
        $('#pop').modal('show');

    })
    $('body').on('click', '#btn_chitiet', function () {
        let ma = $(this).attr('data-id')
        window.location.href = 'Index';
        $('#btn_ma').val(ma);
    })
    $('body').on('click', '#btn_updatehd', function () {
        let ma = $(this).attr('data-id')
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                $('#btn_save').show();
                $('#pop').modal('show');
            }
            else {
                alert(re.message)
            }
        })
    })

    $('body').on('click', '#btn_save', function () {

        $.post('/Update-hoadon', { hd: getdata() }, function (re) {

            if (re.status) {
                alert(re.message)
                $('#pop').modal('hide');
                window.location.reload();
            }
            else {
                alert(re.message)
            }
        })
    
    })

})
function setdata(data) {
    if (data == null || data == undefined || data == '') {
        $('#btn_ma').val('');
        $('#btn_UserID').val('');
        $('#btn_MaVoucher').val('');
        $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
        $('#btn_TenKhachHang').val('');
        $('#btn_SoDienThoai').val('');
        $('#btn_PhiShip').val('');
        $('#btn_TongGiaTriHangHoa').val('');
        $('#btn_PhuongTTT').val('');
        $('#btn_Status').val('');
    }
    else {
        var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD');
        $('#btn_ma').val(data.maHoaDon);
        $('#btn_UserID').val(data.userID);
        $('#btn_MaVoucher').val(data.maVoucher);
        $('#btn_NgayTaoDon').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
        $('#btn_TenKhachHang').val(data.tenKhachHang);
        $('#btn_SoDienThoai').val(data.soDienThoai);
        $('#btn_PhiShip').val(data.phiShip);
        $('#btn_TongGiaTriHangHoa').val(data.tongGiaTriHangHoa);
        $('#btn_PhuongTTT').val(data.phuongThucThanhToan);
        $('#btn_Status').val(data.trangThai);
    }
}
function getdata() {
    return {
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(),
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        TenKhachHang: $('#btn_TenKhachHang').val(),
        SoDienThoai: $('#btn_SoDienThoai').val(),
        PhiShip: $('#btn_PhiShip').val(),
        TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
        PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
        TrangThai: $('#btn_Status').val(),
    }
}
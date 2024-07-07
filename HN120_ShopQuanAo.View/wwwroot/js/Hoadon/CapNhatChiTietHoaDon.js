﻿$(document).ready(function () {
    $('body').on('click', '#btn_xemchitiet', function () {
        var url = $(this).data('url');
        window.location.href = url;
    })
    $('body').on('click', '#btn_themchitietsp', function () {
       fakedata();
        $('#pop_sp').modal('show');
    })

    $('body').on('click', '#btn_updatedhct', function () {

        var btn = parseFloat($(this).attr('data-GiaBan'));
        var ten = $(this).attr('data-tensp');
        var sku = $(this).attr('data-SKU');
        var img = $(this).attr('data-img');
        var tt = $(this).attr('data-trangthai');
        let mahoadon = $('#btn_mahoadon').val();
        $('#pop_sp').modal('hide');
        $('#pop_soluongmua').modal('show');
        $('#btn_themsoluong').off('click').on('click', function () {
            let soluong = parseFloat($('#inp_soluongmua').val());
            if (isNaN(soluong) || soluong == 0 || soluong == undefined) {
                $('#pop_soluongmua').modal('hide');
                $.notify('Vui lòng nhập số lượng (số lượng phải > 0)', 'error');
                return;
            }
            else {
                let dg = soluong * btn;
                let tong = dg + parseInt($('#btn_TongGiaTriHangHoa_fake').val());
                $('#tongtiennew_fake').val(tong);
                var data_ = {
                    //MaHoaDonChiTiet: ma,
                    UrlAnhSpct: img,
                    SKU: sku,
                    MaHoaDon: mahoadon,
                    TenSp: ten,
                    SoLuongMua: soluong,
                    DonGia: dg,
                    TrangThai: tt,

                };
                $.ajax({
                    url: '/Add-hoadonct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify([data_]),
                    success: function (re) {
                        if (re.status) {
                            var datahoadon = getdatafake();
                            $.post('/Update-hoadon', { hd: datahoadon }, function (re) {

                                if (re.status) {
                                    localStorage.setItem('notification', JSON.stringify({ message: 'Thêm sản phẩm thành công', type: 'success' }))
                                    $('#pop_soluongmua').modal('hide');
                                    window.location.reload();
                                }
                                else {
                                    $.notify('Có lỗi sảy ra khi cập nhật, vui lòng kiểm tra, hoặc gọi cho đội ngũ phát triển', 'error')
                                }
                            })
                           
                        } else {
                            $.notify('Thêm thất bại', 'error')
                        }
                    },
                    error: function (error) {
                        alert('Có lỗi xảy ra khi gửi dữ liệu.');
                    }
                });
            }

        })
    });

    $('body').on('click', '#btn_xoachitietsp', function () {
        fakedata();
        let ma = $(this).attr('data-ma')
        $.post('/Dell-HDCT/' + ma, function (re) {
            if (re.status) {             
                var tong = parseInt($('#btn_TongGiaTriHangHoa_fake').val()) - re.data.donGia
                $('#tongtiennew_fake').val(tong);
                var datahoadon = getdatafake();
                $.post('/Update-hoadon', { hd: datahoadon }, function (re) {

                    if (re.status) {
                        localStorage.setItem('notification', JSON.stringify({ message: 'Đã xóa sản phẩm', type: 'success' }))
                        window.location.reload();
                    }
                    else {
                        $.notify('Có lỗi sảy ra khi cập nhật, vui lòng kiểm tra, hoặc gọi cho đội ngũ phát triển', 'error')
                    }
                })
               
            }
            else {
                $.notify('Xóa thất bại', 'error');
            }
        })

    })
    $('body').on('click', '#btn_update_hoadon', function () {
        fakedata(); 
    }) 
  
})
function fakedata() {
    let ma = $('#btn_mahoadon').val();
    $.get('/Detail-hoadon/' + ma, function (re) {
        if (re.status) {
            setdatafake(re.data)
        }
        else {
            alert(re.message)
        }
    })
}
function setdatafake(data) {
    var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD HH:mm:ss');
    $('#btn_ma_fake').val(data.maHoaDon);
    $('#btn_UserID_fake').val(data.userID);
    $('#btn_MaVoucher_fake').val(data.maVoucher);
    $('#btn_NgayTaoDon_fake').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
    $('#btn_TenKhachHang_fake').val(data.tenKhachHang);
    $('#btn_SoDienThoai_fake').val(data.soDienThoai);
    $('#btn_PhiShip_fake').val(data.phiShip);
    $('#btn_TongGiaTriHangHoa_fake').val(data.tongGiaTriHangHoa);
    $('#btn_PhuongTTT_fake').val(data.phuongThucThanhToan);
    $('#btn_Status_fake').val(data.trangThai);
    $('#btn_phanloai_fake').val(data.phanLoai);
    $('#btn_ghichu_fake').val(data.ghichu);
    $('#street_fake').val(data.cuthe);
    $('#city_fake').val(data.tinhThanh);
    $('#district_fake').val(data.quanHuyen);
    $('#ward_fake').val(data.xaPhuong);
}
function getdatafake() {
    return {
        MaHoaDon: $('#btn_ma_fake').val(),
        UserID: $('#btn_UserID_fake').val(),
        MaVoucher: $('#btn_MaVoucher_fake').val(),
        NgayTaoDon: $('#btn_NgayTaoDon_fake').val(),
        TenKhachHang: $('#btn_TenKhachHang_fake').val(),
        SoDienThoai: $('#btn_SoDienThoai_fake').val(),
        PhiShip: $('#btn_PhiShip_fake').val(),
        TongGiaTriHangHoa: $('#tongtiennew_fake').val(),
        PhuongThucThanhToan: $('#btn_PhuongTTT_fake').val(),
        TrangThai: $('#btn_Status_fake').val(),
        PhanLoai: $('#btn_phanloai_fake').val(),
        TinhThanh: $('#city_fake').val(),
        QuanHuyen: $('#district_fake').val(),
        XaPhuong: $('#ward_fake').val(),
        Cuthe: $('#street_fake').val(),
        Ghichu: $('#btn_ghichu_fake').val(),
    }
}
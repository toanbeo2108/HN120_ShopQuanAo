$(document).ready(function () {
    $('body').on('click', '#btn_xemchitiet', function () {
        var url = $(this).data('url');
        window.location.href = url;
    })
    $('body').on('click', '#btn_themchitietsp', function () {
        
       fakedata();
        $('#pop_sp').modal('show');
    })
 
    $('body').on('click', '#btn_updatedhct', function () {
        var productData = {
         btn : parseFloat($(this).attr('data-GiaBan_')),
         ten : $(this).attr('data-tensp_'),
         sku : $(this).attr('data-SKU_'),
         masp : $(this).attr('data-MaSp_'),
         tensz : $(this).attr('data-tensz_'),
         masz_ : $(this).attr('data-masize_'),
         Tenmau : $(this).attr('data-Tenmau_'),
         MaMau_ : $(this).attr('data-mamau_'),
         MaKhuyenMai : $(this).attr('data-makm_'),
         img : $(this).attr('data-img_'),
         DonGia_ : parseFloat($(this).attr('data-dongia_')),
         GiaBan : parseFloat($(this).attr('data-GiaBan_')),
         mahoadon : $('#btn_mahoadon').val(),
         SoLuongTon : parseInt($(this).attr('data-SoLuongTon_')),
         tt : parseInt($(this).attr('data-trangthai_'))

        }
       
        $('#btn_nhap').val(productData.sku);
       
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
               

                let dg = soluong * productData.btn;
                let tong = dg + parseInt($('#btn_TongGiaTriHangHoa_fake').val());
                $('#tongtiennew_fake').val(tong);
                var skuToFind = $('#btn_nhap').val(); // Thay thế bằng mã SKU của bạn
                var foundRow = null;
                $("#hoaDonctTable tbody tr").each(function () {
                    var sku_ = $(this).find("td.sku").text().trim();
                    if (sku_ === skuToFind) {
                        foundRow = $(this);
                        return false; // Dừng vòng lặp khi tìm thấy
                    }                  
                });
                if (foundRow) {
                    var maHoaDonChiTiet = foundRow.find("input.maHoaDonChiTiet").val();
                    var sku = foundRow.find("input.sku").val();
                    var maHoaDon = foundRow.find("input.maHoaDon").val();
                    var tenSp = foundRow.find("input.tenSp").val();
                    var donGia = foundRow.find("input.donGia").val();
                    var soLuongMua = foundRow.find("input.soLuongMua").val();
                    var trangThai = foundRow.find("input.trangThai").val();

                    var masp        = foundRow.find("input.masp").val();
                    var masz        = foundRow.find("input.masz").val();
                    var MaMau        = foundRow.find("input.MaMau").val();
                    var MaKhuyenMai = foundRow.find("input.MaKhuyenMai").val();
                    var img         = foundRow.find("input.img").val();
                    var DonGia_     = foundRow.find("input.DonGia_").val();
                    var GiaBan      = foundRow.find("input.GiaBan").val();
                    var SoLuongTon   = foundRow.find("input.SoLuongTon").val();
                    var tt          = foundRow.find("input.tt").val();



                    var arrayhdct = [];
                    var dt = [{
                        MaHoaDonChiTiet: maHoaDonChiTiet,
                        SKU: sku,
                        MaHoaDon: maHoaDon,
                        TenSp: tenSp,
                        DonGia: (parseFloat(donGia) + dg),
                        SoLuongMua: (parseInt(soLuongMua) + soluong),
                        TrangThai: parseInt(trangThai),

                    }];
                   var dt2 = {
                        SKU: sku,
                        MaSp: masp,
                        MaSize: masz,
                        MaMau: MaMau,
                        MaKhuyenMai: MaKhuyenMai,
                        UrlAnhSpct: img,
                        Dongia: parseFloat(DonGia_),
                        GiaBan: parseFloat(GiaBan),
                        SoLuongTon: (parseInt(SoLuongTon) - soluong),
                        TrangThai: parseInt(tt),
                    };
                    $.ajax({
                        url: '/Update-hoadonct',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(dt),
                        success: function (response) {
                            if (response.status) {
                                $.ajax({
                                    url: '/Update_soluongCTsanpham',
                                    method: 'POST',
                                    contentType: 'application/json',
                                    dataType: 'json',
                                    data: JSON.stringify([dt2]),
                                    success: function (re) {
                                        if (re.status) {
                                            localStorage.setItem('notification', JSON.stringify({ message: 'Cập nhật thông tin hóa đơn thành công', type: 'success' }));
                                            window.location.reload();
                                        } else {
                                            console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                                        }
                                    },
                                    error: function () {
                                        console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                                    }
                                });
                                
                            } else {
                                $.notify("Đã xảy ra lỗi: " + err, 'error');
                            }
                        }
                    });
                }
                else {
                    let mahoadon = $('#btn_mahoadon').val();
                    var data_ = {
                        UrlAnhSpct: productData.img,
                        SKU: productData.sku,
                        MaHoaDon: mahoadon,
                        TenSp: productData.ten,
                        SoLuongMua: soluong,
                        DonGia: dg,
                        TrangThai: productData.tt,
                    };
                    var datasp = {
                        SKU: productData.sku,
                        MaSp: productData.masp,
                        MaSize: productData.masz_,
                        MaMau: productData.MaMau_,
                        MaKhuyenMai: productData.MaKhuyenMai,
                        UrlAnhSpct: productData.img,
                        DonGia: productData.DonGia_,
                        GiaBan: productData.GiaBan,
                        SoLuongTon: (productData.SoLuongTon - soluong),
                        TrangThai: productData.tt
                    }

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
                                        $.ajax({
                                            url: '/Update_soluongCTsanpham',
                                            method: 'POST',
                                            contentType: 'application/json',
                                            dataType: 'json',
                                            data: JSON.stringify([datasp]),
                                            success: function (re) {
                                                if (re.status) {
                                                    localStorage.setItem('notification', JSON.stringify({ message: 'Thêm sản phẩm thành công', type: 'success' }))
                                                    $('#pop_soluongmua').modal('hide');
                                                    window.location.reload();
                                                } else {
                                                    console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                                                }
                                            },
                                            error: function () {
                                                console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                                            }
                                        });
                                        
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
            }

        })
    });

    $('body').on('click', '#btn_xoachitietsp', function () {
        fakedata();
        var rowCount = 0;

        // Lặp qua tất cả các hàng trong tbody và đếm chúng
        $("#hoaDonctTable tbody tr").each(function () {
            rowCount++;
        });
        if (rowCount >=2 ) {
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
        }
        else {
            $.notify('Phải có sản phẩm trong hóa đơn', 'error');
            return;
        }
       

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

function getdataahoadonchitiet_() {
    var hdctList = [];
    $('#hoaDonctTable tbody tr').each(function () {
        var maHoaDonChiTiet = $(this).find('.maHoaDonChiTiet').val();
        var sku = $(this).find('.sku').val();
        var maHoaDon = $(this).find('.maHoaDon').val();
        var tenSp = $(this).find('.tenSp').val();
        var donGia = parseFloat($(this).find('.donGia').val());
        var soLuongMua = parseInt($(this).find('.soLuongMua').val());
        var trangThai = parseInt($(this).find('.trangThai').val());

        var dt = {
            MaHoaDonChiTiet: maHoaDonChiTiet,
            SKU: sku,
            MaHoaDon: maHoaDon,
            TenSp: tenSp,
            DonGia: donGia,
            SoLuongMua: soLuongMua,
            TrangThai: trangThai
        };
        hdctList.push(dt);
    });
    return hdctList;
}
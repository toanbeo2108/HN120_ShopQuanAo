
$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    }
    $('#btn_ngaytao').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    $('#btn_ngaythaydoils').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    $('#btn_Update').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    $('#ngayupdate_tthd').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    let ma = $('#btn_mahoadon').val();
    $('#GetAllHoaDon_partialView').show();
    $('#HoaDonTaiQuayPartialView').hide();
    $('#BanHangOnline_partialView').hide();
    $('body').on('change','#btn_phanloaihoadon',function () {
        if ($('#btn_phanloaihoadon').val() == '') {
            $('#GetAllHoaDon_partialView').show();
            $('#HoaDonTaiQuayPartialView').hide();
            $('#BanHangOnline_partialView').hide();
        }
        if ($('#btn_phanloaihoadon').val() == 1) {
            $('#HoaDonTaiQuayPartialView').show();
            $('#GetAllHoaDon_partialView').hide();
            $('#BanHangOnline_partialView').hide();
        }
        if ($('#btn_phanloaihoadon').val() == 2) {
            $('#BanHangOnline_partialView').show();
            $('#GetAllHoaDon_partialView').hide();
            $('#HoaDonTaiQuayPartialView').hide();  
        }
       
    });
    
   

    $('body').on('click', '#btn_update_hoadon', function () {
        let ma = $('#btn_mahoadon').val();
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)               
                $('#pop_updatehoadon').modal('show');
            }
            else {
                alert(re.message)
            }
        })
    }) 
    $('body').on('click', '#btn_close', function(){
        setdata(null);    
         $('#pop_updatehoadon').modal('hide');
    })
    $('body').on('click', '#btn_save', function () {
        let ship = parseFloat($('#btn_PhiShip').val());
        let ship_fake = parseFloat($('#btn_PhiShip_fake').val());
        let thaydoitienship = ship - ship_fake;
        let tongtien = parseFloat( $('#btn_TongGiaTriHangHoa_fake').val());
        let tongtiensaukhithaydoi = tongtien + thaydoitienship;
        if (tongtiensaukhithaydoi <= 0) {
            tongtiensaukhithaydoi = 0;
        }
        $('#btn_TongGiaTriHangHoa').val(tongtiensaukhithaydoi);
        updatehoadon();   
        var today = new Date();

        var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
        var tongtienhoadon = $('#btn_tonggiatrihd').val().replace(/\./g, '').split(',')[0];
        var data = {
            LichSuHoaDonID: date,
            MaHoaDon: $('#btn_mahoadonls').val(),
            UserID: $('#btn_UserName').val(),
            NgayTaoDon: $('#btn_ngaytao').val(),
            NgayThayDoi: $('#btn_ngaythaydoils').val(),
            TongGiaTri: parseFloat(tongtienhoadon),
            HinhThucThanhToan: $('#btn_httt').val(),
            ChiTiet: $('#btn_ghichu').val(),
            TrangThai: 7,
        };

        $.post('/Add-lichsuhoadon', { lshd: data }, function (re) {
            if (re.status) {

            }
            else {

            }

        })
    }) 

    var token = '8fbfedf6-b458-11ee-b6f7-7a81157ff3b1';
    async function loadTinhThanh() {
        var tinhthanh = $('#city').val();
        let provinceID;
        let districtID;
        let wardCode;
        try {
            const response = await $.ajax({
                url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province',
                method: 'GET',
                headers: {
                    'Token': token
                }
            });

            response.data.forEach(function (item) {
                if (item.ProvinceName.trim() === tinhthanh.replace(/^Tỉnh\s+|\s+$/g, '').trim()) { // So sánh chuỗi đã loại bỏ khoảng trắng thừa và dấu tiếng Việt
                    provinceID = item.ProvinceID;
                }
            });
            await loadQuanHuyen(provinceID)
        } catch (error) {
            alert('Không tìm thấy mã tỉnh.');
        }
        async function loadQuanHuyen(provinceId) {
            var quanhuyen = $('#district').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district',
                    method: 'GET',
                    headers: {
                        'Token': token
                    }
                });

                response.data.forEach(function (item) {
                    if (item.DistrictName.trim() === quanhuyen.trim()) {

                        districtID = item.DistrictID;
                    }

                });
                await loadXaPhuong(districtID);
            } catch (error) {
                handleError(error);
            }
        }
        async function loadXaPhuong(districtID) {
            var xaphuong = $('#ward').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward',
                    method: 'GET',
                    headers: {
                        'Token': token
                    },
                    data: { district_id: districtID }
                });

                response.data.forEach(function (item) {
                    console.log('So sánh:', item.WardName.trim(), '===', xaphuong.trim()) // Hiển thị để kiểm tra so sánh

                    if (item.WardName.trim() === xaphuong.trim()) {

                        wardCode = item.WardCode;
                    }
                });
                //if (wardCode == undefined) {
                //  $.notify('Hiện giao hàng nhanh chưa hỗ trợ địa điểm này, thông cảm !', 'error') 
                //}
                console.log('Mã Tỉnh' + provinceID + 'Mã quận: ' + districtID + 'Mã xã: ' + wardCode);
                await getShippingFee(provinceID, districtID, wardCode)
            } catch (error) {

            }
        }
        async function getShippingFee(provinceID, districtID, wardCode) {
            const SshopId = 192652;
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Token': token
                        //'ShopId': SshopId
                    },
                    data: JSON.stringify({
                        "service_type_id": 2,
                        "from_district_id": 3440,
                        "from_ward_code": "13009",
                        "to_district_id": districtID,
                        "to_ward_code": wardCode,
                        "height": 20,
                        "length": 30,
                        "weight": 3000,
                        "width": 40,
                        "insurance_value": 0,
                        "coupon": null

                    })
                });

                if (response.code === 200) {

                    $('#btn_PhiShip').val(response.data.total);
                    updatePaymentDetails()

                } else {
                    console.error('Error:', response.message);
                }
            } catch (error) {
                console.error('Error calling API:', error);
            }
        }
    }
    $('#ward').change(function () {
        loadTinhThanh();
    })
   
    $('body').on('click', '#btn_Xacnhandonhang', function () {
        
        
        updateHoaDon(2);
        AddLichsuhoadon(2);
        let datasp = getSanPhamChiTiet();
        $.ajax({
            url: '/Update_soluongCTsanpham',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(datasp),
            success: function (re) {
                if (re.status) {
                    console.log('Trừ số lượng sản phẩm thành công');
                } else {
                    console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                }
            },
            error: function () {
                console.error('Có lỗi xảy ra khi gửi yêu cầu.');
            }
        });
    })
    $('body').on('click', '#btn_ChoGiaoHang', function () {
        updateHoaDon(3);
        AddLichsuhoadon(3);
    })
    $('body').on('click', '#btn_GiaoHang', function () {
        updateHoaDon(4);
        AddLichsuhoadon(4);
    })
    $('body').on('click', '#btn_HoanThanh', function () {
        updateHoaDon(5);
        AddLichsuhoadon(5);
        AddThanhToanHoaDon_();
    })
 
    $('body').on('click', '#btn_back', function () {
        let stt = $('#stt_hoadon').val();
       
        updateHoaDon(stt - 1);
        AddLichsuhoadon(stt - 1);
        if (stt == 2) {
            let datasp = getSanPhamChiTiet();
            $.ajax({
                url: '/Update_soluongCTsanpham',
                method: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(datasp),
                success: function (re) {
                    if (re.status) {
                        console.log('Trừ số lượng sản phẩm thành công');
                    } else {
                        console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                    }
                },
                error: function () {
                    console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                }
            });
        }
       
    })
    $('body').on('click', '#btn_huydon', function () {
        $('#table_hoadon tbody tr').each(function () {

            var stt = $(this).find('#stt_hoadon').val();
            $('#stthoadon_fake').val(stt)
        });
        $('#pop_huydon').modal('show');
       
    })
    $('body').on('click', '#btn_dongy', function () {
        if ($('#btn_ghichuhuy').val() == '') {
            $.notify('Vui lòng nhập ghi chú', 'error');
        }
        else {
            var today = new Date();

            var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
            var tongtienhoadon = $('#btn_tonggiatrihd').val().replace(/\./g, '').split(',')[0];
            var data = {
                LichSuHoaDonID: date,
                MaHoaDon: $('#btn_mahoadonls').val(),
                UserID: $('#btn_nhanvien').val(),
                NgayTaoDon: $('#btn_ngaytao').val(),
                NgayThayDoi: $('#btn_ngaythaydoils').val(),
                TongGiaTri: parseFloat(tongtienhoadon),
                HinhThucThanhToan: $('#btn_httt').val(),
                ChiTiet: $('#btn_ghichuhuy').val(),
                TrangThai: parseInt($('#btn_trangthai').val())
            };

            $.post('/Add-lichsuhoadon', { lshd: data }, function (re) {
                if (re.status) {
                    updateHoaDon(6);
                    window.location.reload();
                }
                else {
                    $.notify('Lưu hóa đơn thất bại', 'error');
               
                }

            })
        }
       
        if ($('#stthoadon_fake').val() == 2 || $('#stthoadon_fake').val() == 3) {
            let datasp = getSanPhamChiTiet();
            $.ajax({
                url: '/Update_soluongCTsanpham',
                method: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(datasp),
                success: function (re) {
                    if (re.status) {
                        console.log('Trừ số lượng sản phẩm thành công');
                    } else {
                        console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                    }
                },
                error: function () {
                    console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                }
            });
        }
    });
    $('body').on('click', '#btn_xemlichsu', function () {
        $('#pop_lshd').modal('show');
    })
    //var dtspct = getSanPhamChiTiet();
    //console.log(dtspct);
})

function updateHoaDon(status) {
    let ma = $('#btn_mahoadon').val();
    $.get('/Detail-hoadon/' + ma, function (re) {
        if (re.status) {
            setdata(re.data);
            let data = {
                MaHoaDon: $('#btn_mahoadon').val(),
                UserID: $('#btn_UserID').val(),
                MaVoucher: $('#btn_MaVoucher').val(),
                NgayTaoDon: $('#btn_NgayTaoDon').val(),
                NgayThayDoi: $('#btn_Update').val(),
                TenKhachHang: $('#btn_TenKhachHang').val(),
                SoDienThoai: $('#btn_SoDienThoai').val(),
                PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                TrangThai: status,
                PhanLoai: $('#btn_phanloai').val(),
                TinhThanh: $('#city').val(),
                QuanHuyen: $('#district').val(),
                XaPhuong: $('#ward').val(),
                Cuthe: $('#street').val(),
                Ghichu: $('#btn_ghichu').val(),
            };

            $.post('/Update-hoadon', { hd: data }, function (re) {
                if (re.status) {
                    localStorage.setItem('notification', JSON.stringify({ message: 'Cập nhật thông tin hóa đơn thành công', type: 'success' }));
                    window.location.reload();
                } else {
                    $.notify('Có lỗi xảy ra khi cập nhật, vui lòng kiểm tra, hoặc gọi cho đội ngũ phát triển', 'error');
                }
            });
        } else {
            alert(re.message);
        }
    });
}

function updateHoaDonChiTiet(datahdct) {
    $.ajax({
        url: '/Update-hoadonct',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(datahdct),
        success: function (response) {
            if (response.status) {
                localStorage.setItem('notification', JSON.stringify({ message: 'Cập nhật thông tin hóa đơn thành công', type: 'success' }));
                window.location.reload();
            } else {
                $.notify("Đã xảy ra lỗi: " + err, 'error');
            }
        }
    });
}

function updatehoadon() {
    var hoadon = getdatahoadon();
    console.log(hoadon);
    $.post('/Update-hoadon', { hd: getdatahoadon() }, function (re) {

        if (re.status) {
            localStorage.setItem('notification', JSON.stringify({ message: 'Cập nhật thông tin hóa đơn thành công', type: 'success' }))
            $('#pop').modal('hide');
            window.location.reload();
        }
        else {
            $.notify('Có lỗi sảy ra khi cập nhật, vui lòng kiểm tra, hoặc gọi cho đội ngũ phát triển', 'error')
        }
    })
}
function setdata(data) {  
    var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD HH:mm:ss');
    $('#btn_mahoadon').val(data.maHoaDon);
      $('#btn_UserID').val(data.userID);
        $('#btn_MaVoucher').val(data.maVoucher);
        $('#btn_NgayTaoDon').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
        // $('#btn_Update').val(moment().format('YYYY-MM-DD HH:mm:ss'));
        $('#btn_TenKhachHang').val(data.tenKhachHang);
        $('#btn_SoDienThoai').val(data.soDienThoai);
        $('#btn_PhiShip').val(data.phiShip);
        $('#btn_TongGiaTriHangHoa').val(data.tongGiaTriHangHoa);
        $('#btn_PhuongTTT').val(data.phuongThucThanhToan);
        $('#btn_Status').val(data.trangThai);
        $('#btn_phanloai').val(data.phanLoai);
        $('#btn_ghichu').val(data.ghichu);
        $('#street').val(data.cuthe);
        document.getElementById('city').innerHTML = '<option value="' + data.tinhThanh + '">' + data.tinhThanh + '</option>';
        document.getElementById('district').innerHTML = '<option value="' + data.quanHuyen + '">' + data.quanHuyen + '</option>';
        document.getElementById('ward').innerHTML = '<option value="' + data.xaPhuong + '">' + data.xaPhuong + '</option>';
        
        
}
function getdatahoadon() {

    return {
        MaHoaDon: $('#btn_mahoadon').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(),
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        NgayThayDoi: $('#btn_Update').val(),
        TenKhachHang: $('#btn_TenKhachHang').val(),
        SoDienThoai: $('#btn_SoDienThoai').val(),
        PhiShip: parseFloat($('#btn_PhiShip').val()) != '' ? parseFloat($('#btn_PhiShip').val()) : 0,
        TongGiaTriHangHoa: parseFloat($('#btn_TongGiaTriHangHoa').val()),
        PhuongThucThanhToan: parseInt($('#btn_PhuongTTT').val()),
        TrangThai: parseInt($('#btn_Status').val()),
        PhanLoai: $('#btn_phanloai').val(),
        TinhThanh: $('#city').val(),
        QuanHuyen: $('#district').val(),
        XaPhuong: $('#ward').val(),
        Cuthe: $('#street').val(),
        Ghichu: $('#btn_ghichu').val(),
    }
}
//function getdataahoadonchitiet() { 
//   var hdctList = [];
//    $('#hoaDonctTable tbody tr').each(function () {
//        var maHoaDonChiTiet = $(this).find('.maHoaDonChiTiet').val();
//        var sku = $(this).find('.sku').val();
//        var maHoaDon = $(this).find('.maHoaDon').val();
//        var tenSp = $(this).find('.tenSp').val();
//        var donGia = parseFloat($(this).find('.donGia').val());
//        var soLuongMua =parseInt($(this).find('.soLuongMua').val());
//        var trangThai = parseInt($(this).find('.trangThai').val());

//        var masp = $(this).find('.masp').val();
//        var masz = $(this).find('.masz').val();
//        var MaMau = $(this).find('.MaMau').val();
//        var MaKhuyenMai = $(this).find('.MaKhuyenMai').val();
//        var img = $(this).find('.img').val();
//        var DonGia_ = parseFloat($(this).find('.DonGia_').val());
//        var GiaBan = parseFloat($(this).find('.GiaBan').val());
//        var SoLuongTon = parseInt($(this).find('.SoLuongTon').val());
//        var tt = parseInt($(this).find('.tt').val());

//        var dt = {
//            MaHoaDonChiTiet: maHoaDonChiTiet,
//            SKU: sku,
//            MaHoaDon: maHoaDon,
//            TenSp: tenSp,
//            DonGia: donGia,
//            SoLuongMua: soLuongMua,
//            TrangThai: trangThai,

//            MaSp: masp,
//            MaSize: masz,
//            MaMau: MaMau,
//            MaKhuyenMai: MaKhuyenMai,
//            UrlAnhSpct: img,
//            Dongia_: parseFloat(DonGia_),
//            GiaBan: parseFloat(GiaBan),
//            SoLuongTon: (parseInt(SoLuongTon) - soLuongMua),
//            TrangThai: parseInt(tt),
//        };
//    hdctList.push(dt);
//    });
//    return hdctList;
//}
function getSanPhamChiTiet() {
    var SPCT = [];
    $('#hoaDonctTable tbody tr').each(function () {
        
        var sku = $(this).find('input.sku').val();
        var soLuongMua = parseInt($(this).find('.soLuongMua').val());
        var trangThai = parseInt($(this).find('.trangThai').val());

        var masp = $(this).find('.masp').val();
        var masz = $(this).find('.masz').val();
        var MaMau = $(this).find('.MaMau').val();
        var MaKhuyenMai = $(this).find('.MaKhuyenMai').val();
        var img = $(this).find('.img').val();
        var DonGia_ = parseFloat($(this).find('input.DonGia_').val());
        var GiaBan = parseFloat($(this).find('input.GiaBan').val());
        var SoLuongTon = parseInt($(this).find('.SoLuongTon').val());
        var tt = parseInt($(this).find('.tt').val());
        let stt = $('#stt_hoadon').val();
        let sttf = $('#stthoadon_fake').val();
        let soluongupdate;
        if (stt == 1) {
            soluongupdate = (parseInt(SoLuongTon) - soLuongMua);
        }
        if (stt == 2) {
            soluongupdate = (parseInt(SoLuongTon) + soLuongMua);
        }
        if (sttf == 2 || sttf == 3 ) {
            soluongupdate = (parseInt(SoLuongTon) + soLuongMua);
        }
        var dt = {
            SKU: sku,
            MaSp: masp,
            MaSize: masz,
            MaMau: MaMau,
            MaKhuyenMai: MaKhuyenMai,
            UrlAnhSpct: img,
            Dongia: DonGia_,
            GiaBan: GiaBan,
            SoLuongTon: soluongupdate,
            TrangThai: parseInt(tt),
        };
        SPCT.push(dt);
    });
    return SPCT;
}

function AddLichsuhoadon(status) {
    var today = new Date();

    var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
    var tongtienhoadon = $('#btn_tonggiatrihd').val().replace(/\./g, '').split(',')[0];
    var data = {
        LichSuHoaDonID: date,
        MaHoaDon: $('#btn_mahoadonls').val(),
        UserID: $('#btn_UserName').val(),
        NgayTaoDon: $('#btn_ngaytao').val(),
        NgayThayDoi: $('#btn_ngaythaydoils').val(),
        TongGiaTri: parseFloat(tongtienhoadon),
        HinhThucThanhToan: $('#btn_httt').val(),
        ChiTiet: status,
        TrangThai: status,
    };

    $.post('/Add-lichsuhoadon', { lshd: data }, function (re) {
        if (re.status) {
            
        }
        else {
          
        }

    })
}
function AddThanhToanHoaDon_() {
    var data;
    $('#tb_thanhtoan tbody tr').each(function () {
        
        var _MaPhuongThuc_HoaDon = $(this).find('.MaPhuongThuc_HoaDon_').text();
        var _MaHoaDon = $(this).find('.MaHoaDon_').text();
        var _MaPhuongThuc = $(this).find('.MaPhuongThuc_').text();
        var _NgayTao = $(this).find('.NgayTao_').text();
        var _NgayThayDoi = $(this).find('.NgayThayDoi_').text();
        var _TrangThai = $(this).find('.tt').text();
        var today = $('#ngayupdate_tthd').val();
   
         data = {
            MaPhuongThuc_HoaDon: _MaPhuongThuc_HoaDon,
            MaHoaDon: _MaHoaDon,
            MaPhuongThuc: _MaPhuongThuc ,
            NgayTao: _NgayTao,
            NgayThayDoi: today,
            TrangThai: 1
            };
    })
    $.post('/Update-thanhtoanhoadon', { tt: data }, function (re) {
        if (re.status) {
            console.log('Thanh toán thành công')
        }
        else {
            console.log('Lưu thanh toán hóa đơn thất bại');
        }

    })
}
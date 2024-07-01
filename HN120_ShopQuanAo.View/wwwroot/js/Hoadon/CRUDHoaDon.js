//import { error } from "console";

$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    }
    let ma = $('#btn_mahoadon').val();
    $('#GetAllHoaDon_partialView').show();
    $('#HoaDonTaiQuayPartialView').hide();
    $('#BanHangOnline_partialView').hide();
    $('#Tatca').click(function () {
        $('#GetAllHoaDon_partialView').show();
        $('#HoaDonTaiQuayPartialView').hide();
        $('#BanHangOnline_partialView').hide();
    });
    
    $('#bantaiquay').click(function () {
        $('#HoaDonTaiQuayPartialView').show();
        $('#GetAllHoaDon_partialView').hide();
        $('#BanHangOnline_partialView').hide();

    });

    $('#banonline').click(function () {
        $('#BanHangOnline_partialView').show();
        $('#GetAllHoaDon_partialView').hide();
        $('#HoaDonTaiQuayPartialView').hide();
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
        let ma = $('#btn_mahoadon').val();
        var datahdct = getdataahoadonchitiet();

        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                var data = {
                    MaHoaDon: $('#btn_mahoadon').val(),
                    UserID: $('#btn_UserID').val(),
                    MaVoucher: $('#btn_MaVoucher').val(),
                    NgayTaoDon: $('#btn_NgayTaoDon').val(),
                    TenKhachHang: $('#btn_TenKhachHang').val(),
                    SoDienThoai: $('#btn_SoDienThoai').val(),
                    PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                    TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                    PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                    TrangThai: 2,
                    PhanLoai: $('#btn_phanloai').val(),
                    TinhThanh: $('#city').val(),
                    QuanHuyen: $('#district').val(),
                    XaPhuong: $('#ward').val(),
                    Cuthe: $('#street').val(),
                    Ghichu: $('#btn_ghichu').val(),
                }                 
                $.post('/Update-hoadon', { hd: data }, function (re) {
                    if (re.status) {
                        
                        $.ajax({
                            url: '/Update-hoadonct',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify(datahdct),
                            success: function (response) {
                               
                                if (response.status) {
                                    localStorage.setItem('notification', JSON.stringify({ message: 'Cập nhật thông tin hóa đơn thành công', type: 'success' }))
                                  
                                    window.location.reload();
                                } else {
                                    $.notify("Đã xảy ra lỗi: " + err, 'error');
                                }
                            },                           
                        }); 
                    }
                    else {
                        $.notify('Có lỗi sảy ra khi cập nhật, vui lòng kiểm tra, hoặc gọi cho đội ngũ phát triển', 'error')
                    }
                })
            }
            else {
                $.notify('Có lỗi xảy ra: ' + error, 'error');
            }
        })

        
    })
    $('body').on('click', '#btn_ChoGiaoHang', function () {
        let ma = $('#btn_mahoadon').val();
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                var data = {
                    MaHoaDon: $('#btn_mahoadon').val(),
                    UserID: $('#btn_UserID').val(),
                    MaVoucher: $('#btn_MaVoucher').val(),
                    NgayTaoDon: $('#btn_NgayTaoDon').val(),
                    TenKhachHang: $('#btn_TenKhachHang').val(),
                    SoDienThoai: $('#btn_SoDienThoai').val(),
                    PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                    TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                    PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                    TrangThai: 3,
                    PhanLoai: $('#btn_phanloai').val(),
                    TinhThanh: $('#city').val(),
                    QuanHuyen: $('#district').val(),
                    XaPhuong: $('#ward').val(),
                    Cuthe: $('#street').val(),
                    Ghichu: $('#btn_ghichu').val(),
                }
                $.post('/Update-hoadon', { hd: data }, function (re) {

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
            else {
                alert(re.message)
            }
        })
    })
    $('body').on('click', '#btn_GiaoHang', function () {
        let ma = $('#btn_mahoadon').val();
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                var data = {
                    MaHoaDon: $('#btn_mahoadon').val(),
                    UserID: $('#btn_UserID').val(),
                    MaVoucher: $('#btn_MaVoucher').val(),
                    NgayTaoDon: $('#btn_NgayTaoDon').val(),
                    TenKhachHang: $('#btn_TenKhachHang').val(),
                    SoDienThoai: $('#btn_SoDienThoai').val(),
                    PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                    TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                    PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                    TrangThai: 4,
                    PhanLoai: $('#btn_phanloai').val(),
                    TinhThanh: $('#city').val(),
                    QuanHuyen: $('#district').val(),
                    XaPhuong: $('#ward').val(),
                    Cuthe: $('#street').val(),
                    Ghichu: $('#btn_ghichu').val(),
                }
                $.post('/Update-hoadon', { hd: data }, function (re) {

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
            else {
                alert(re.message)
            }
        })
    })
    $('body').on('click', '#btn_HoanThanh', function () {
        let ma = $('#btn_mahoadon').val();
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                var data = {
                    MaHoaDon: $('#btn_mahoadon').val(),
                    UserID: $('#btn_UserID').val(),
                    MaVoucher: $('#btn_MaVoucher').val(),
                    NgayTaoDon: $('#btn_NgayTaoDon').val(),
                    TenKhachHang: $('#btn_TenKhachHang').val(),
                    SoDienThoai: $('#btn_SoDienThoai').val(),
                    PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                    TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                    PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                    TrangThai: 5,
                    PhanLoai: $('#btn_phanloai').val(),
                    TinhThanh: $('#city').val(),
                    QuanHuyen: $('#district').val(),
                    XaPhuong: $('#ward').val(),
                    Cuthe: $('#street').val(),
                    Ghichu: $('#btn_ghichu').val(),
                }
                $.post('/Update-hoadon', { hd: data }, function (re) {

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
            else {
                alert(re.message)
            }
        })
    })
    $('body').on('click', '#btn_HoanThanh', function () {
        fakedata();
        $('#btn_Status').val(5);
        //  updatehoadon_fake();
    })
    $('body').on('click', '#btn_back', function () {
        let ma = $('#btn_mahoadon').val();
        $.get('/Detail-hoadon/' + ma, function (re) {
            if (re.status) {
                setdata(re.data)
                let stt = parseInt($('#btn_Status').val());
                var data = {
                    MaHoaDon: $('#btn_mahoadon').val(),
                    UserID: $('#btn_UserID').val(),
                    MaVoucher: $('#btn_MaVoucher').val(),
                    NgayTaoDon: $('#btn_NgayTaoDon').val(),
                    TenKhachHang: $('#btn_TenKhachHang').val(),
                    SoDienThoai: $('#btn_SoDienThoai').val(),
                    PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
                    TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
                    PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
                    TrangThai: (stt -1),
                    PhanLoai: $('#btn_phanloai').val(),
                    TinhThanh: $('#city').val(),
                    QuanHuyen: $('#district').val(),
                    XaPhuong: $('#ward').val(),
                    Cuthe: $('#street').val(),
                    Ghichu: $('#btn_ghichu').val(),
                }
                $.post('/Update-hoadon', { hd: data }, function (re) {

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
            else {
                alert(re.message)
            }
        })
    })
})
function updatehoadon() {
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
        var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD');
    $('#btn_mahoadon').val(data.maHoaDon);
        $('#btn_UserID').val(data.userID);
        $('#btn_MaVoucher').val(data.maVoucher);
        $('#btn_NgayTaoDon').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
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
        TenKhachHang: $('#btn_TenKhachHang').val(),
        SoDienThoai: $('#btn_SoDienThoai').val(),
        PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
        TongGiaTriHangHoa: $('#btn_TongGiaTriHangHoa').val(),
        PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
        TrangThai: $('#btn_Status').val(),
        PhanLoai: $('#btn_phanloai').val(),
        TinhThanh: $('#city').val(),
        QuanHuyen: $('#district').val(),
        XaPhuong: $('#ward').val(),
        Cuthe: $('#street').val(),
        Ghichu: $('#btn_ghichu').val(),
    }
}
function getdataahoadonchitiet() { 
   var hdctList = [];
    $('#hoaDonctTable tbody tr').each(function () {
        var maHoaDonChiTiet = $(this).find('.maHoaDonChiTiet').val();
        var sku = $(this).find('.sku').val();
        var maHoaDon = $(this).find('.maHoaDon').val();
        var tenSp = $(this).find('.tenSp').val();
        var donGia = parseFloat($(this).find('.donGia').val());
        var soLuongMua =parseInt($(this).find('.soLuongMua').val());
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
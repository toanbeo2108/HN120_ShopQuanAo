//import { Console } from "console";

$(document).ready(function () {
    var token = 'd0d7cce1-3125-11ef-8e53-0a00184fe694';
    filterFunction();
    addSelectButtonEventListeners();
    $('#btn_SoDienThoai').on('change', validatePhoneNumber);
    var today = new Date();

    var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
    $('#btn_ma').val(date.toString());
    $('#sanpham_table tbody tr').each(function () {
        var giaBanCell = $(this).find('td:eq(3)');

        var giaBanText = giaBanCell.html().split('<br>')[0].trim();
        var giaBan = parseFloat(giaBanText.replace(' VNĐ', '').replace(/\./g, '').replace(',', '.'));
      //  var giaBan = parseFloat(giaBanCell.text().trim().replace(/\./g, '').replace(',', '.'));
        if (!isNaN(giaBan)) {
            // giaBanCell.text(formatMoney(giaBan));
            giaBanCell.html(formatMoney(giaBan) + ' VNĐ<br>' + giaBanCell.html().split('<br>')[1]);
        }
    });
    $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
    $('body').on('click', '#btn_thanhtoanhoadon', function () {
        var today = new Date();

        var date = 'HD' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        $('#btn_ma').val(date.toString());
        $('#btn_Status').val(5);

        let tt = ($('#btn_tienkhachdua').val().replace(/\./g, '').replace(',', '.'));
        let kt = ($('#btn_tienkhachphaitra').val().replace(/\./g, '').replace(',', '.'));
        
        if ((tt - kt >= 0) || tt == '' || tt == '' || tt == 0) {
            if ($('#btn_phuongthucthanhtoan').val() =='TT1') {
               
                Thanhtoan();
            }
            if ($('#btn_phuongthucthanhtoan').val() == 'TT2') {
              
                QRCODE_PAYMENT();
            }
                    
        }
        if ((tt - kt <= 0) && $('#btn_phuongthucthanhtoan').val() == 'TT3') {

            QRCODE_PAYMENT();
        }     
    });
    async function loadTinhThanh() {
        var tinhthanh = $('#city').val();
        let provinceID;
        let districtID;
        let wardCode = null;
        try {
            const response = await $.ajax({
                url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province',
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
                    url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district',
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
                    url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward',
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
                console.log('Mã Tỉnh' + provinceID + 'Mã quận: ' + districtID + 'Mã xã: ' + wardCode);
                await getShippingFee(provinceID, districtID, wardCode)
            } catch (error) {

            }
        }
        async function getShippingFee(provinceID, districtID, wardCode) {
            const SshopId = 192652;
            try {
                const response = await $.ajax({
                    url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Token': token,
                        'ShopId': SshopId
                    },
                    data: JSON.stringify({
                        "service_type_id": 2,
                        "from_district_id": 3440,
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
    $('#btn_MaVoucher').change(function () {       
        updatePaymentDetails();  
    });
    $('#btn_PhiShip').change(function () {
        updatePaymentDetails();
    });
    $('#btn_MaVoucher').change(updatePaymentDetails);
    $('#btn_PhiShip').on('input', updatePaymentDetails);
    $('#btn_chonsp').click(updatePaymentDetails);

    // Lắng nghe thay đổi số lượng sản phẩm trong bảng chi tiết hóa đơn (giả sử có input số lượng)
    $('#chitiethoadon_table').on('input', 'input[type="number"]', updatePaymentDetails);
    //formatPriceColumn();
    
    $('#btn_tienkhachdua').on('input', function () {
        updatePaymentDetails();
    });
    $('#btn_truvoucher').on('change', function () {
        updatePaymentDetails();
    });
    $('#btn_tienkhachdua').change(function () {

        tienthua();
    });
    $('#btn_tienkhachdua').on('focus', handleTienKhachDuaFocus);
    $('body').on('click', '#btn_dathang', function () {
       $('#btn_Status').val(1)
        Thanhtoan();
        
    })

    $('#btn_TenKhachHang').change(function () {
        var selectedOption = this.options[this.selectedIndex];
        var tinhThanh = selectedOption.getAttribute('data-tinh');
        var quanHuyen = selectedOption.getAttribute('data-quanhuyen');
        var xaPhuong = selectedOption.getAttribute('data-xaphuong');
        var cuThe = selectedOption.getAttribute('data-cuthe');
        if ($('#btn_btn_TenKhachHang').val != '') {
            document.getElementById('city').innerHTML = '<option value="' + tinhThanh + '">' + tinhThanh + '</option>';
            document.getElementById('district').innerHTML = '<option value="' + quanHuyen + '">' + quanHuyen + '</option>';
            document.getElementById('ward').innerHTML = '<option value="' + xaPhuong + '">' + xaPhuong + '</option>';
            document.getElementById('street').value = cuThe;
        }
        loadTinhThanh();
    });
});


// thanh toán chuyển khoản 
function QRCODE_PAYMENT() {
    $('#pop_QR').modal('show');
    let countdownTime = 120;
    const countdownElement = document.getElementById("countdown");

    // Cập nhật đồng hồ đếm ngược mỗi giây
    const countdownInterval = setInterval(function () {
        countdownElement.textContent = countdownTime + " seconds remaining";
        countdownTime--;
        // Khi đồng hồ đếm ngược về 0, đóng modal và dừng đồng hồ đếm ngược
        if (countdownTime < 0) {
            clearInterval(countdownInterval);
            alert('Giao dịch không thành công, quá thời gian chờ')
            $("#pop_QR").modal('hide');
            window.location.reload();
        }
    }, 1000); // Cập nhật mỗi giây
    $('#btn_sotienck').text(($('#btn_tienkhachphaitra').val() != '' ? $('#btn_tienkhachphaitra').val() : 0) + ' VNĐ');

    thongtinhoadon = $('#btn_thongtinhoadon').val();
    //string thongtinhoadon = a;
    let sotienck = parseInt($('#btn_tienkhachphaitra').val().replace('.',''));
    let QR = `https://img.vietqr.io/image/MB-0336262156-qr_only.png?amount=${sotienck}&addInfo=${thongtinhoadon}`
    $('#imgQR').attr('src', QR);
    setInterval(() => {

        checkpaid(sotienck, thongtinhoadon);
    }, 1000);
}

async function checkpaid(price , content) {
    try {
        const respon = await fetch("https://script.google.com/macros/s/AKfycbxwZpNWFZTZmzDCaWaxfbHbUX5hHKImRf1bCtbIU8dQkHKmDNhagjFAxkR9TDLGMoMYKQ/exec");
        const datas = await respon.json();

        if (datas.error === false) {
            const lastPaid = datas.data[0];
            let pr = lastPaid['Giá trị'];
            let ct = lastPaid['Mô tả'];           
            if (pr >= price && ct.includes(content)) {                
                Thanhtoan(); 
            }
            else {
                console.log('giao dịch không thành công'); 
            }
        } else {
            console.error("Error in the response:", datas.error);
        }
    } catch (e) {

    }
}
function tienthua() {
    var tienkhachdua = $('#btn_tienkhachdua').val();
    var tienkhachphaitra = $('#btn_tienkhachphaitra').val()
    
    var value = formatMoney(parseFloat(tienkhachdua))
    var value2 = parseFloat(tienkhachphaitra.replace(/\./g, '').replace(',', '.'));

    $('#btn_tienkhachdua').val(value);
    if ($('#btn_phuongthucthanhtoan').val() == 'TT1') {

        let tienthua = tienkhachdua - value2;
        if (tienkhachdua <= 0 || tienkhachdua < value2) {
            $('#btn_tienthua').val(0);
        } else {
            $('#btn_tienthua').val(formatMoney(tienthua));
        }
    }
     if ($('#btn_phuongthucthanhtoan').val() == 'TT3') {
         let tienchuyenkhoan = value2 - tienkhachdua;     
         let tck = formatMoney(tienchuyenkhoan);
         $('#btn_tienkhachphaitra').val(tck)
    } 
   
}

function handleTienKhachDuaFocus() {
    var input = $('#btn_tienkhachdua');
    input.val(''); // Reset giá trị của input
}

// kiểm tra số điện thoại
function validatePhoneNumber() {
    var phoneInput = $('#btn_SoDienThoai');
    var phoneError = $('#phoneError');
    var phoneNumber = phoneInput.val();

    // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
    var phonePattern = /^0\d{9,10}$/;

    // Kiểm tra tính hợp lệ của số điện thoại
    if (!phonePattern.test(phoneNumber)) {
        phoneError.show();
    } else {
        phoneError.hide();
    }
}

// format money 
function formatMoney(amount) {
    return amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, '.').replace('.', ',').replace(/(\d+),(\d+)$/, '$1,$2').replace(/,/g, '.').replace(/(\d+)\.(\d+)$/, '$1,$2')
}
function filterFunction() {
    var searchInput = document.getElementById("search-input").value.toUpperCase();
    var selectedColor = document.getElementById("select-color").value;
    var selectedSize = document.getElementById("select-size").value;

    var table = document.getElementById("sanpham_table");
    var rows = table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var color = row.querySelector('[data-mamau]').getAttribute('data-mamau').trim();
        var size = row.querySelector('[data-masize]').getAttribute('data-masize').trim();
        var tenSp = row.querySelector('[data-tensp]').getAttribute('data-tensp').toUpperCase().trim();

        if ((selectedColor === "Chọn màu" || color === selectedColor) &&
            (selectedSize === "Chọn size" || size === selectedSize) &&
            (tenSp.includes(searchInput))) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    }
}


// Function to add event listeners to select buttons
function addSelectButtonEventListeners() {
    var selectButtons = document.querySelectorAll('#sanpham_table .btn_chonsp');

    selectButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var sku = button.getAttribute('data-sku');
            var tenSp = button.getAttribute('data-tensp');
            var giaBan = parseFloat(button.getAttribute('data-giaban'));
            var maSize = button.getAttribute('data-masize');
            var maMau = button.getAttribute('data-mamau');
            var slton = parseInt(button.getAttribute('data-soluongton'));

            //if (slton === 0) {
            //    alert('Sản phẩm đã hết hàng, không thể thêm vào đơn hàng.');
            //    return;
            //}

            addToInvoiceDetails(sku, tenSp, giaBan, maSize, maMau, slton);
        });
    });
}

function addToInvoiceDetails(sku, tenSp, giaBan, maSize, maMau, slton) {
    var table = document.getElementById("chitiethoadon_table");
    if (!table) return; // Kiểm tra table có tồn tại hay không

    var tbody = table.getElementsByTagName("tbody")[0];
    if (!tbody) return; // Kiểm tra tbody có tồn tại hay không

    var rows = tbody.getElementsByTagName("tr");

    // Kiểm tra nếu sản phẩm đã tồn tại trong bảng chi tiết hóa đơn
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        if (row.cells.length > 0 && row.cells[0].innerText === sku && row.cells[1].innerText === tenSp && row.cells[2].innerText === maSize && row.cells[3].innerText === maMau) {
            // Tăng số lượng mua
            var soLuongInput = row.cells[4].querySelector('input');
            
            if (soLuongInput) {
                var soLuongMua = parseInt(soLuongInput.value);
                if (soLuongMua + 1 > slton) {
                    alert('Số lượng mua vượt quá số lượng tồn kho.');
                    return;
                }
                soLuongInput.value = soLuongMua + 1;
              
                updateTotalPrice(soLuongInput, giaBan, slton);
                    updatePaymentDetails();
                
            }
            return; // Dừng lại sau khi cập nhật
        }
    }

    // Nếu sản phẩm chưa tồn tại, thêm mới vào bảng
    addNewRow(tbody, sku, tenSp, giaBan, maSize, maMau, slton);
    updatePaymentDetails();
}

// Function to add a new row to invoice details table
function addNewRow(table, sku, tenSp, giaBan, maSize, maMau, slton) {
    var newRow = table.insertRow();
    var skucell = newRow.insertCell(0);
    var tenSpCell = newRow.insertCell(1);
    var maSizeCell = newRow.insertCell(2);
    var maMauCell = newRow.insertCell(3);
    var soLuongCell = newRow.insertCell(4);
    var giaBanCell = newRow.insertCell(5);
    var actionCell = newRow.insertCell(6);

    skucell.innerText = sku;
    tenSpCell.innerText = tenSp;
    maSizeCell.innerText = maSize;
    maMauCell.innerText = maMau;
    soLuongCell.innerHTML = '<input type="number" value="1" min="1" class="form-control" oninput="updateTotalPrice(this, ' + giaBan + ', ' + slton + ')">';

    giaBanCell.innerText = formatMoney(giaBan);
    actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="removeRow(this)"><i class="bi bi-trash"></i></button>';
    updatePaymentDetails();
}
// Function to update total price based on quantity change
function updateTotalPrice(input, giaBan, slton ) {
    var row = input.closest('tr');
    if (!row) return; // Kiểm tra row có tồn tại hay không
    var soLuongMua = parseInt(input.value);
    if (soLuongMua > slton) {
        alert('Số lượng mua vượt quá số lượng khả dụng.');
        input.value = slton;
        soLuongMua = slton;
    }
    var totalPriceCell = row.cells[5];
    var soLuongInput = row.cells[4].querySelector('input'); // kiểm tra số lượng mua
    if (!totalPriceCell) return; // Kiểm tra totalPriceCell có tồn tại hay không
    totalPriceCell.innerText = formatMoney(parseFloat(input.value) * giaBan);  
    updatePaymentDetails();
}

// Function to remove a row from invoice details table
function removeRow(button) {
    var row = button.closest('tr');
    if (row) {
        row.remove();
        updatePaymentDetails();
    }
}
// Function to update payment details (total, discount, customer payment, change)
function updatePaymentDetails() {
    var total = 0;
    $('#chitiethoadon_table tbody tr').each(function () {
        // var soLuong = parseFloat($(this).find('input[type="number"]').val());
        //var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.')); 
        var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.'));

        if (!isNaN(giaBan)) {
            total += giaBan;
        }
    });
    
    var ship = ($('#btn_PhiShip').val() === '') ? 0 : parseFloat($('#btn_PhiShip').val());
    var selectedOption = $('#btn_MaVoucher').find(':selected');
    var giaTriGiam = selectedOption.val() !== '' ? parseFloat(selectedOption.data('giatrigiam').replace(/\./g, '').replace(',', '.')) : 0;
   /* var tienkhachdua = ($('#btn_tienkhachdua').val() == '') ? 0 : parseFloat($('#btn_tienkhachdua').val());*/
    var tongtienkhachphaitra = total + ship - giaTriGiam;
    var tongtien = total;
    if (tongtienkhachphaitra <= 0) {
        $('#btn_tienkhachphaitra').val(0);
    }
    else {
        $('#btn_tienkhachphaitra').val(formatMoney(tongtienkhachphaitra));
    }   
    $('#btn_tongtien').val(formatMoney(tongtien));
}

  
function getdataHoaDonChiTiet() {
    
    var hoaDonChiTiets = [];
    let mahd = $('#btn_ma').val();
   
    $('#chitiethoadon_table tbody tr').each(function () {
        // Kiểm tra xem dòng này có cells không
        if ($(this).find('td').length > 0) {
            var sku = $(this).find('td:eq(0)').text().trim();
            var tenSp = $(this).find('td:eq(1)').text().trim();
            var maSize = $(this).find('td:eq(2)').text().trim();
            var maMau = $(this).find('td:eq(3)').text().trim();
            var soLuongMua = parseInt($(this).find('td:eq(4) input').val().trim());
            var donGia = parseFloat($(this).find('td:eq(5)').text().trim());

            var hoaDonChiTiet = {
               // MaHoaDonChiTiet: $('#btn_ma').val(),
                SKU: sku,
                TenSp: tenSp,
                MaHoaDon: mahd,
                DonGia: donGia,
                SoLuongMua: soLuongMua,
                TrangThai: 1
            };

            hoaDonChiTiets.push(hoaDonChiTiet);
        }
    });

    return hoaDonChiTiets;
}
function setdataHoaDon(data) {
    if (data == null || data == undefined || data == '') {
        $('#btn_ma').val('');
        $('#btn_UserID').val('');
        $('#btn_MaVoucher').val('');
        $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
        $('#btn_TenKhachHang').val('');
        $('#btn_SoDienThoai').val('');
        $('#btn_PhiShip').val('');
        $('#btn_tongtien').val('');
        $('#btn_PhuongTTT').val('');
        $('#btn_Status').val(5);    
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
        $('#btn_tongtien').val(data.tongGiaTriHangHoa);
        $('#btn_PhuongTTT').val(data.phuongThucThanhToan);
        $('#btn_Status').val(data.trangThai);
    }
}
function getdataHoaDon() {

    return {
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(),
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        TenKhachHang: $('#btn_TenKhachHang').val(),
        SoDienThoai: $('#btn_SoDienThoai').val(),
        PhiShip: $('#btn_PhiShip').val(),
        TongGiaTriHangHoa: $('#btn_tongtien').val(),
        PhuongThucThanhToan: $('#btn_phuongthucthanhtoan').val(),
        TrangThai: $('#btn_Status').val(),
    }                                           
}
function Thanhtoan() {
    var hoaDonChiTiets = getdataHoaDonChiTiet();
    $.post('/Add-hoadon', { hd: getdataHoaDon() }, function (re) {
        if (re.status) {
            console.log(getdataHoaDon());
            $.ajax({
                url: '/Add-hoadonct',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(hoaDonChiTiets),
                success: function (re) {
                    if (re.status) {      
                        if ($('#btn_Status').val() == 5) {
                            alert('Thanh toán thành công');
                            window.location.reload();
                        }
                         if ($('#btn_Status').val() == 1) {
                             alert('Tạo đơn thành công');
                             window.location.reload();
                        }

                           
                    } else {
                        alert(re.message);
                    }
                },
                error: function (xhr, status, error) {
                    alert('Có lỗi xảy ra: ' + error);
                }
            });
        }
        else {
            alert(re.message);
        }
    }).fail(function (xhr, status, error) {
        // Xử lý lỗi khi yêu cầu /Add-hoadon thất bại
        alert('Có lỗi xảy ra khi thêm hóa đơn: ' + error);
    });
}

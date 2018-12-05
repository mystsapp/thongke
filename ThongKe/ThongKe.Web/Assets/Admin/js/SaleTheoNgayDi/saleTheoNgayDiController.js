$.validator.addMethod("dateFormat",
    function (value, element) {
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split('/');
            var dd = parseInt(adata[0], 10);
            var mm = parseInt(adata[1], 10);
            var yyyy = parseInt(adata[2], 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd)) {
                check = true;
            }
            else {
                check = false;
            }
        } else {
            check = false;
        }
        return this.optional(element) || check;
    },
    "Chưa đúng định dạng dd/mm/yyyy.");

var saleTheoNgayDiController = {
    init: function () {
        // saleTheoNgayDiController.LoadData();
        saleTheoNgayDiController.loadDdlDaiLy();
        saleTheoNgayDiController.registerEvent();
    },

    registerEvent: function () {

        //$('.modal-dialog').draggable();

        $('#frmSearch').validate({
            rules: {
                tungay: {
                    required: true,
                    //date: true
                    dateFormat: true
                },
                denngay: {
                    required: true,
                    //date: true
                    dateFormat: true
                }
            },
            messages: {
                tungay: {
                    required: "Trường này không được để trống.",
                    //date: "Chưa đúng định dạng."
                },
                denngay: {
                    required: "Trường này không được để trống.",
                    //number: "password phải là số",
                    //date: "Chưa đúng định dạng."
                }
            }
        });


        $('#btnSearch').off('click').on('click', function () {
            $('#frmSearch').submit();
        });

        $('#btnReset').off('click').on('click', function () {
            saleTheoNgayDiController.resetForm();
        });

        $("#txtTuNgay, #txtDenNgay").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd/mm/yy"

        });
    },
    resetForm: function () {
        $('#txtTuNgay').val('');
        $('#txtDenNgay').val('');
    },
    loadDdlDaiLy: function () {
        
        $('#ddlDaiLy').html('');
        var cn = $('#hidCn').data('cn');
        //alert(cn);
        var option = '';
        // option = option + '<option value=select>Select</option>';

        $.ajax({
            url: '/account/GetDmdailyByChiNhanh',
            type: 'GET',
            data: {
                chinhanh: cn
            },
            dataType: 'json',
            success: function (response) {
                //if (response.length > 0) {
                //var data = JSON.stringify(response.data);
                var data = JSON.parse(response.data);
                console.log(data);

                $.each(data, function (i, item) {
                    option = option + '<option value="' + item.Daily + '">' + item.Daily + '</option>'; //chinhanh1

                });
                $('#ddlDaiLy').html(option);
            }
        });
        //homeController.resetForm();
        //var id = $('.btn-edit').data('id');

    }

}
saleTheoNgayDiController.init();
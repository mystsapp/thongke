var tuyenTheoNgayController = {
    init: function () {
        // tuyenTheoNgayController.LoadData();
        tuyenTheoNgayController.registerEvent();
    },

    registerEvent: function () {

        $('#frmSearch').validate({
            rules: {
                tungay: {
                    required: true,
                    date: true
                },
                denngay: {
                    required: true,
                    date: true
                }
            },
            messages: {
                tungay: {
                    required: "Trường này không được để trống.",
                    date: "Chưa đúng định dạng."
                },
                denngay: {
                    required: "Trường này không được để trống.",
                    //number: "password phải là số",
                    date: "Chưa đúng định dạng."
                }
            }
        });


        $('#btnSearch').off('click').on('click', function () {
            $('#frmSearch').submit();
        });

        $('#btnReset').off('click').on('click', function () {
            tuyenTheoNgayController.resetForm();
        });

        $("#txtTuNgay, #txtDenNgay").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "mm/dd/yy"

        });
    },
    resetForm: function () {
        $('#txtTuNgay').val('');
        $('#txtDenNgay').val('');
    }

}
tuyenTheoNgayController.init();
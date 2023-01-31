const main = (function () {

    function CreateDatatable(id, options) {
        var defaultOptions = {
            destroy: true,
            info: true,
            responsive: true,
            pageLength: 10,
            "lengthMenu": [[10, 20, 30, 50, 100], [10, 20, 30, 50, 100]],
            sDom: '<"row view-filter"<"col-sm-12"<"float-left"l><"float-right"f><"clearfix">>>t<"row view-pager"<"datatablePages"<"text-center"ip>>>',
            language: {
                url: "/_admin/js/Turkish.json",

                paginate: {
                    previous: "<i class='ph-arrow-circle-left'></i>",
                    next: "<i class='simple-icon-arrow-right'></i>"
                }
            },
            drawCallback: function () {
                $($(".dataTables_wrapper .pagination li:first-of-type"))
                    .find("a")
                    .addClass("prev");
                $($(".dataTables_wrapper .pagination li:last-of-type"))
                    .find("a")
                    .addClass("next");

                $(".dataTables_wrapper .pagination").addClass("pagination-sm");
                $('.view-filter').remove();
                $('.datatable-outofbox-infotext').text($('.dataTables_info').text());
                $('.dataTables_info').hide();
            }
        };

        var settings = $.extend({}, defaultOptions, options);

        var oTable = $(id).DataTable(settings);

        $('#datatableSearch').keyup(function () {

            var val = $(this).val();
            oTable.search(val).draw();
        });

        $('.datatable-outofbox-lengthtext').on('change', function (e) {
            var val = $(this).val();
        });


        $('.datatable-outofbox-dropdownButton').click(function () {
            $('.datatable-outofbox-lengthtext').text($(this).text());
            oTable.page.len($(this).text()).draw();
        });


        return oTable;
    }

    return {
        CreateDatatable: CreateDatatable,
    };

}());
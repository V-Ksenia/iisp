$(document).ready(function () {
    $(document).on('click', '.page-link', function (event) {
        event.preventDefault();
        var url = $(this).attr('href');

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                $('#product-container').html(result);
            }
        }).fail(function (error) {
            console.error("Ошибка при загрузке данных: ", error);
        });
    });
});

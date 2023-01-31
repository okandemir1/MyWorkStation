const layout = (function () {

    const swalInit = swal.mixin({
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger',
            denyButton: 'btn btn-danger',
            input: 'form-control'
        }
    });

    function success_message(title, message) {
        if (title == null || title.length <= 0) { title = "islem Basarili"; }
        if (message == null || message.length <= 0) { message = "islem basarili"; }

        swalInit.fire(
            title,
            message,
            'success'
        )
    }

    function error_message(title,message) {
        if (title == null || title.length <= 0) { title = "Hay Aksi"; }
        if (message == null || message.length <= 0) { message = "Basarisiz islem"; }

        swalInit.fire(
            title,
            message,
            'error'
        )
    }

    function error_message_list(title, message, errors) {
        let html = "", item;
        html += '<p>' + message + '</p>';
        if (errors.length > 0) {
            html += '<ul style="list-style:none;margin:0;padding:0;">';

            for (k in errors) {
                item = errors[k];
                html += '<li style="margin-bottom:5px;">' + item + '</li>';
            }

            html += '</ul>';
        }

        swalInit.fire({
            title: title,
            icon: 'warning',
            html: html,
            showCloseButton: true,
            confirmButtonText: 'Tamam',
            confirmButtonAriaLabel: 'Tamam',
        });
    }

    function success_action_message(title, message, href) {
        let timerInterval;
        if (message.length <= 0) { message = "Ýþlem Tamamlandý"; }
        if (title.length <= 0) { title = "Ýþlem Baþarýlý"; }
        swalInit.fire({
            title: title,
            html: message,
            icon: 'success',
            confirmButtonText: 'Tamam',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading()
                timerInterval = setInterval(() => {
                    const content = Swal.getHtmlContainer()
                    if (content) {
                        const b = content.querySelector('b')
                        if (b) {
                            b.textContent = Swal.getTimerLeft()
                        }
                    }
                }, 100)
            },
            willClose: () => {
                clearInterval(timerInterval);
                if (href.length <= 0) location.reload();
                location.href = href;
            }
        })
    }

    function error_action_message(title, message, href) {
        if (message.length <= 0) { message = "Bir eksiklik var"; }
        if (title.length <= 0) { title = "Hay Aksi!"; }

        swalInit.fire({
            title: title,
            html: message,
            timer: 5000,
            icon: 'error',
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading()
                timerInterval = setInterval(() => {
                    const content = Swal.getHtmlContainer()
                    if (content) {
                        const b = content.querySelector('b')
                        if (b) {
                            b.textContent = Swal.getTimerLeft()
                        }
                    }
                }, 100)
            },
            willClose: () => {
                clearInterval(timerInterval);
                if (href.length <= 0) location.reload();
                location.href = href;
            }
        });
    }

    const ajaxHelper = (settings, callback) => {
        let returnOBJ = {};
        $.ajax(settings)
            .done(function (e) {
                returnOBJ.error = false;
                returnOBJ.code = "";
                returnOBJ.data = e;
                callback(e);
            })
            .fail(function (e) {
                returnOBJ.error = true;
                returnOBJ.code = "1";
                returnOBJ.data = "";
                callback(e);
            });
    };

    function show_loading() { $(".loading").css("display", "flex"); }
    function hide_loading() { $(".loading").hide(); }

    function convertToSlug(Text) {
        return Text
            .toLowerCase()
            .replaceAll("ç", "c")
            .replaceAll("ð", "g")
            .replaceAll("ý", "i")
            .replaceAll("ö", "o")
            .replaceAll("þ", "s")
            .replaceAll("ü", "u")
            .replace(/[^\w ]+/g, '')
            .replace(/ +/g, '-');
    }

    return {
        ajaxHelper: ajaxHelper,
        success_message: success_message,
        error_message: error_message,
        error_message_list: error_message_list,
        success_action_message: success_action_message,
        error_action_message: error_action_message,
        show_loading: show_loading,
        hide_loading: hide_loading,
        convertToSlug: convertToSlug,
    }
}());
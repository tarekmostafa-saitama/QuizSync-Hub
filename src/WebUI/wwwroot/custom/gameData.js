var ldld = new ldLoader({ root: "#my-loader" });




//theme submission
document.querySelector('#themeForm').addEventListener('submit',
    (e) => {
        e.preventDefault();
        var id = $('#Id').val(); 
        var url =`/Admin/Games/${id}/Theme`
       
        var formData = new FormData(document.querySelector('#themeForm'));
        ldld.on();

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            success: function (data) {
                ldld.off();

                setTimeout(function () {
                    location.reload();
                },
                    2000);
            },
            cache: false,
            contentType: false,
            processData: false
        });
    }
);


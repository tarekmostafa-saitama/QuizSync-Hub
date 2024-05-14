var ldld = new ldLoader({ root: "#my-loader" });


function DeleteConfirmation(url, reload = true, method = 'GET', removedElementId = null,
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning") {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then(function (choice) {
        if (choice.value) {
            ldld.on();
            $.ajax({
                url: url,
                type: method,
                success: function (result) {
                    ldld.off();
                    if (result.success) {
                        Swal.fire(result.message);
                        if (reload)
                            location.reload(true);
                        if (removedElementId) {
                            if (document.getElementById(removedElementId))
                                document.getElementById(removedElementId).remove();
                        }
                    }
                    else {
                        Swal.fire(result.message);
                    }
                },
                error: function (xhr) {
                    ldld.off;
                    alert('Something seems Wrong');
                }
            });
        }
    });

}

function OperationConfirmation(url, reload = true, method = 'GET',
    title = "Are you sure ?",
    text = "Are you sure you want to perform this operation ?",
    icon = "warning") {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then(function (choice) {
        if (choice.value) {
            ldld.on();
            $.ajax({
                url: url,
                type: method,
                success: function (result) {
                    ldld.off();
                    if (result.success) {
                        Swal.fire(result.message);
                        if (reload)
                            setTimeout(() => {
                                    location.reload(true);
                                },
                                2000);
                    }
                    else {
                        Swal.fire(result.message);
                    }
                },
                error: function (xhr) {
                    ldld.off;
                    alert('Something seems Wrong');
                }
            });
        }
    });

}




var setInnerHTML = function (elm, html) {
    elm.innerHTML = html;
    Array.from(elm.querySelectorAll("script")).forEach(oldScript => {
        const newScript = document.createElement("script");
        Array.from(oldScript.attributes)
            .forEach(attr => newScript.setAttribute(attr.name, attr.value));
        newScript.appendChild(document.createTextNode(oldScript.innerHTML));
        oldScript.parentNode.replaceChild(newScript, oldScript);
      
    });
}

function OpenPopupContent(url, title) {
    ldld.on();
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        processData: false,
        contentType: false,
        success: function (result) {
            ldld.off();
            document.getElementById("generalModalTitle").textContent = title;
            setInnerHTML(document.getElementById("generalModalBody"), result);
            $('#generalModal').modal("show");
            $.validator.unobtrusive.parse($("#generalModal form"));

        },
        error: function (e) {
            ldld.off();
            console.log("ERROR : ", e);
            alert('Something seems Wrong while sending the sending request.')
        }
    });


}

function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(window.location.origin + text).then(function () {
        console.log('Async: Copying to clipboard was successful!');
        Snackbar.show({
            text: 'Url Copied Successfully',
            pos: 'bottom-right',
            actionTextColor: '#fff',
            backgroundColor: '#1abc9c'
        });
    }, function (err) {
        console.error('Async: Could not copy text: ', err);
    });
}


function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = window.location.origin + text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        if (successful) {
            Snackbar.show({
                text: 'Url Copied Successfully',
                pos: 'bottom-right',
                actionTextColor: '#fff',
                backgroundColor: '#1abc9c'
            });
        }
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
        console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
}

// sorting

const sortingContainerElm = ".sortable";
var gameId = document.getElementById("Id").value;
console.log(gameId);
const r_pUrl = `/Admin/Games/${gameId}/RearrangeQuestions`;

$(function () {
    $(sortingContainerElm).sortable();
    $(sortingContainerElm).on("sortupdate",
        function (e) {
            sortedElmsOrders = [];
            const sortedElms = Array.from(this.children);
            sortedElms.forEach((elm, i) => {
                const elmId = elm.querySelector(".sortableElmId").value;
                sortedElmsOrders.push({
                    Id: elmId,
                    Order: i + 1
                });
            });
            console.log(sortedElmsOrders);
            // send post request with new orders after sorting
            ldld.on();

            $.ajax({
                url: r_pUrl,
                type: "Post",
                data: JSON.stringify(sortedElmsOrders),
                cache: false,
                processData: false,
                contentType: "application/json",
                success: function (result) {
                    ldld.off();

                    if (result) {
                        Snackbar.show({
                            text: "Order Changed Successfully",
                            actionTextColor: "#fff",
                            backgroundColor: "#1ABC9C"
                        });
                    } else {
                        Snackbar.show({
                            text: "Something seems Wrong while executing the Add request ",
                            actionTextColor: "#fff",
                            backgroundColor: "#E7515A"
                        });
                    }
                },
                error: function (e) {
                    ldld.off();

                    console.log("Error: ", e);
                    alert("Something seems Wrong while sending the Add request.");
                }
            });
        });
});


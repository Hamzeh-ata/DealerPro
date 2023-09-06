$(document).ready(function () {
    $(function () {
        var connection = new signalR.HubConnectionBuilder().withUrl("/cityCenterPCs").build();
        connection.start().then(function () {
            document.getElementById("button").addEventListener("click", function (event) {
                connection.invoke("GetPC")
                    .then(function () {
                        console.log("MyHubMethod invoked successfully.");
                    })
                    .catch(function (err) {
                        console.error(err.toString());
                    });
            });
        }).catch(function (err) {
            console.error(err.toString());
        });
    });

});
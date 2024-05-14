
var ldld = new ldLoader({ root: "#my-loader" });
var pageUrl = window.location.href.split("/");
var gameCode = +pageUrl[5];

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();


  connection.start()   
    .then(() => {
       
        console.log('SignalR Connected!');
        checkGameRunning(); 

    })
    .catch((err) => {

        return console.error(err.toString());
    }); 




function reEnter() {
    location.reload();

}

function checkGameRunning() {
    ldld.on();
    connection.invoke("InitializeGame", gameCode);
}


connection.on("InitializeGame", function (result) {

    if (result.succeeded) {


        Snackbar.show({
            text: "Game Inizlited Successfully",
            actionTextColor: '#fff',
            backgroundColor: '#1abc9c'

        });

    } else {

        $.blockUI({
            message: `<br/><h3>${result.errors[0]}</h3> <br/>`
        });

    }


});


connection.on("GameStarted", function (isVrEnabled) {

        

    $("#moderatorModal").modal('show');


    setTimeout(function () {
        $("#moderatorModal").modal('hide');
       
       
    }, 2000);

    if (isVrEnabled) {

        $('.micIcon').attr("hidden", false);

        createPeerConnection();
    }
});

var timeoutHandle;
function countdown(minutes, seconds) {

    function tick() {

        var counter = document.getElementById("timer");
        counter.innerHTML =
            minutes.toString() + ":" + (seconds < 10 ? "0" : "") + String(seconds);
        seconds--;
        if (seconds < 0 && minutes == 0) {
            changeQuestionIndex(); 
           
        }
        else if (seconds >= 0) { timeoutHandle = setTimeout(tick, 1000); }
        else {
                if (minutes >= 1) {
                // countdown(mins-1);   never reach “00″ issue solved:Contributed by Victor Streithorst
                    setTimeout(function () {
                        countdown(minutes - 1, 59);
                    }, 1000);
                }
             } 
    }
    tick();
}


function changeQuestionIndex() {
    $(".form_content").attr("hidden", true);
    $(".countdown_timer").attr("hidden", true);
    connection.invoke("ChangeCurrentQuestionIndex", gameCode);
}


connection.on("StartTimer", function (question) {

    setTimeout(function () {
        if (question.duration > 60) {
            var minutes = Math.floor(question.duration / 60);
            var seconds = question.duration - minutes * 60;

            countdown(minutes, seconds);

        }
        else {

            countdown(00, question.duration);
        }
    }, 1000);
});

connection.on("CurrentQuestion", function (question , index) {
    ldld.off(); 
    $('.gameContent').attr("hidden", false);


    var img=''; 
    var choice = '';
    if (question.photoUrl != null) {
        img = `
                          <img class="card-img-top img-fluid" src="/uploads/${question.photoUrl}" style="height:250px" alt="Card image cap">

`
    }

    for (var i = 0; i < question.choiceDataModels.length; i++) {
        choice += `
              <li class="list-group-item">- ${question.choiceDataModels[i].choiceText}</li>

`
    }
    $('#currentQuestion').html(`


                    <div class="card">
                     
                          ${img}

                        <div class="card-body">
                            <h5 class="card-text "style="font-weight: bolder">Question : ${question.title} </h5>
                        </div>
                        <hr/>
                        <ul class="list-group list-group-flush">

                                  ${choice}
                        </ul>

                        <div class="text-center mb-2 ms-2">
                            <a onclick="sendQuestion()"  class="btn btn-primary sendQues"> Send  Question </a>
                        </div>
                   </div>
`);



});

connection.on("NotifyOtherInGroupWhenNewJoining", function (name) {

    $('.bodyTable').append(`
     
    <tr class="playersList">
      <td class="playerName playerName_${name.replaceAll(' ', '')}"> ${name}</td>
      <td class="playerScore playerScore_${name.replaceAll(' ', '')}"> 0 </td>
  </tr>
`);
    var oldNum = + $('.players-number').text(); 
    var newNum = oldNum + 1; 

    $('.players-number').text(newNum);

});

connection.on("UpdatePlayerScore", function (score, name) {
    
    $(`.playerScore_${name.replaceAll(' ', '')}`).html(`<p style="font-weight:bolder;"> ${score}</p>`)
    sortTable(); 
});

function sendQuestion() {
    connection.invoke("sendQuestion", gameCode);
    $('.sendQues').attr("hidden", true);
}



function sortTable() {
    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementById("myTable");
    switching = true;
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // Start by saying: no switching is done:
        switching = false;
        rows = document.getElementById("myTable").rows;
        /* Loop through all table rows (except the
        first, which contains table headers): */
        for (i = 1; i < (rows.length - 1); i++) {
            // Start by saying there should be no switching:
            shouldSwitch = false;
            /* Get the two elements you want to compare,
            one from current row and one from the next: */
            x = rows[i].getElementsByTagName("TD")[1];
            y = rows[i + 1].getElementsByTagName("TD")[1];
            // Check if the two rows should switch place:
            if (+(x.innerText) < +(y.innerText)) {
              
                // If so, mark as a switch and break the loop:
                shouldSwitch = true;
              
            }
            

        

            if (shouldSwitch) {
                /* If a switch has been marked, make the switch
                and mark that a switch has been done: */
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
                $("#myTable tbody tr").removeClass('firstScore');
                $("#myTable tbody tr:first").addClass('firstScore');

            }
            if (x.innerText > y.innerText) {
                // If so, mark as a switch and break the loop:
                $("#myTable tbody tr:first").addClass('firstScore');


            } 
        }
       
       
    }
   

}

connection.on("GameEnded", function () {
    $("#timer").attr("hidden", true); 
    $('#currentQuestion').html(`<div class="text-center">
                    <img  src="/userTemplate/assets/images/game_over.png"   alt="image_not_found">
                 
                </div>`);

});

connection.onclose(error => {
    console.log(error);

    $(".gameContent").attr("hidden", true);

    $.blockUI({
        message: `
        <div class="p-5">
            <h3 class="text-danger"> Your're disconnected ! </h3>
            <hr/>
            <p>You are disconnected, please reload the page to start the game again  </p>
            <button type="submit" class="btn btn-info mt-3 text-white" onclick="reEnter()">Re-start The Game</button>
        </div>
        
         <br/>`,
        css: { width: '35rem' }

    });
    disconnected();

});

connection.on("DisconnectedGame", function () {

    disconnected(); 

    $.blockUI({
        message: `<br/><h5> Game Is Disconnected <br/>Please Reload Page if you want to start the game again</h5>  <br/>`,
        css: { width: '35rem' }

    });

});

connection.on("DisconnectedPlayer", function (name) {

    $(`.playerName_${name.replaceAll(' ', '')}`).parent().remove();

    var oldNum = + $('.players-number').text();
    var newNum = oldNum - 1;

    $('.players-number').text(newNum);
});



//#region sound 

var gameStarted = new Audio('/audios/gameStarted.mp3');
var gameCompleted = new Audio('/audios/gameCompleted.mp3');
var losing = new Audio('/audios/losing.mp3');
var trueAnswer = new Audio('/audios/trueAnswer.mp3');
var timeOut = new Audio('/audios/timeOut.mp3'); 
var lastsec = new Audio('/audios/lastSec.mp3'); 
var noramlQues = new Audio('/audios/normalQues.mp3'); 
var doubleScore = new Audio('/audios/doubleScore.mp3'); 
var tripleScore = new Audio('/audios/tripleScore.mp3'); 

gameStarted.volume = 0.1;
gameCompleted.volume = 0.05;
losing.volume = 0.1;
trueAnswer.volume = 0.1;
timeOut.volume = 0.1;
lastsec.volume = 0.1;
noramlQues.volume = 0.1;
doubleScore.volume = 0.1;
tripleScore.volume = 0.1;

//#endregion sound


// #region connecting
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/gameHub")
    .build();
var pageUrl = window.location.href.split("/");
var gameCode = +pageUrl[3]; 


connection.start()
    .then(() => {
        console.log('SignalR Connected!');
        checkGameRunning();

    })
    .catch((err) => {

        return console.error(err.toString());
    });


// #endregion connecting


//#region hub calling

connection.on("CheckGameRunning", function (result) {

    console.log(result)

    if (result.succeeded) {
        $(".login-form").attr("hidden", false);

        console.log("game found..");
    } else {
        $(".login-form").attr("hidden", true);

        $.blockUI({
            message: `<br/><h5>${result.errors[0]}</h5>  <br/>`,
            css: { width: '35rem' }

        });
    }

});


connection.on("JoinPlayer", function (result, name, playersNames) {


    if (result.succeeded) {
        $(".login-form").attr("hidden", true);

        $("#waitingArea").append(`  <div style="margin-top:5rem;" class="col-md-2 mb-4  player_${name}">
         <button class="selfJoining">You </button>     </div>    
        `);
        $(".waiting-area").attr("hidden", false);

        if (playersNames.length > 0) {
            for (var i = 0; i < playersNames.length; i++) {
                $("#waitingArea").append(`  <div style="margin-top:5rem;" class="col-md-2 mb-4 player_${playersNames[i]}">
             <button class="newJoining">${playersNames[i]}</button>     </div>    
            `);
            }
        }

    } else {

        Snackbar.show({
            text: result.errors[0],
            actionTextColor: '#fff',
            backgroundColor: '#e7515a'

        });
    }
 
});


connection.on("NotifyOtherInGroupWhenNewJoining", function (name ) {


    $(".login-form").attr("hidden", true);

    $("#waitingArea").append(` <div style="margin-top:5rem;" class=" col-md-2 mb-4  player_${name.replaceAll(' ', '')}">
     <button class="newJoining">${name}</button>     </div>    
    `);

    $("#waitingArea").attr("hidden", false);


    
});


connection.on("GameStarted", function (isVrEnabled ) {

  
    var modal = new bootstrap.Modal(document.getElementById('gameStartedModal'), {
        keyboard: false
    });
    $("#waitingArea").attr("hidden", true);
    modal.show(); 

    setTimeout(function () {
        
        $(".form_content").attr("hidden", false); 

        modal.hide();

    }, 2000);



    if (isVrEnabled) {
        $('.micIcon').attr("hidden", false);

        createPeerConnection();
    }
}); 
                   

connection.on("CurrentQuestion", function (question, questionIndex) {

    console.log(questionIndex);
    if (questionIndex == 0) {

        gameStarted.play();

        gameStarted.addEventListener('ended', function () {

            getQuestionType(question.score)
        });
    } else {

            getQuestionType(question.score)

    }

    

    $(".form_content").attr("hidden", false);
    $("#waitingArea").attr("hidden", true);
   
  
        var timeleft = question.duration;
        var downloadTimer = setInterval(function () {
    
            if (timeleft <= 0) {
                clearInterval(downloadTimer);
                sendAnswer(null, question.id , 0);

            }

            if (timeleft <= 5 && timeleft>0) {
                lastsec.play();
            }

            if (timeleft < 10) {
                $(".clock_number").text(`0${timeleft}`);
            }

            else {
                $(".clock_number").text(timeleft);

            }
            timeleft -= 1;
        }, 1000);



   
     
        $(".countdown_timer").attr("hidden", false);


    
  


    var img='';
    var choice = '';
    if (question.photoUrl != null) {
        img = `
                           <div class="video_player">
                                 <img src="/uploads/${question.photoUrl}"class="ps-5  pt-3 " style="width: 600px ; height: 250px;  " />                        
                             </div>

`
    }

    for (var i = 0; i < question.choiceDataModels.length; i++) {
        choice += `

                                <label for="opt_${i}" id="textInput" class="step_1 mb-2 position-relative animate__animated animate__fadeInRight animate_25ms ">
                                     - ${question.choiceDataModels[i].choiceText}
                                     <input  id="opt_${i}"  type="radio"  name ="step_1_select_option" value="${question.choiceDataModels[i].choiceText}">
                                 </label>

`


    }
    $('#question-content').html(`

                          ${img}
                           <div class="question_title px-5 pt-3 ">
                                 <h1 name="QuestionTitle">${question.title}</h1>
                             </div>
                             <div class="form_items overflow-hidden pt-5 ps-5">
                                  ${  choice }
                             </div>
      <div class="text-center mt-4">

       <button type="button" id="sendPlayerAnswer" class="btn btn-info rounded-pill  border-0 text-white" >Answer</button>
</div>

`);



    $(".step_1").on('click', function () {
        $(".step_1").removeClass("active");
        $(this).addClass("active");
    });


    $("#sendPlayerAnswer").on('click', function () {
        var answer = $('input[name=step_1_select_option]:checked').val(); 
        var responserTime = +($('.clock_number').text());
        clearInterval(downloadTimer);

        sendAnswer(answer, question.id, responserTime ); 
    });

});


connection.on("WaitingForNextQuestion", function (player, question, isTimeOut) {
    $('.questionType').addClass("imageUnvisible");
    $('.questionType').removeClass("imageVisible");
    var correctAnswer = question.choiceDataModels.filter(function (el) {
        return el.isTrue == true;
    }
    );
    if (isTimeOut == true) {
        timeOut.play();
        $("#waitingArea").html(`

  <div data-aos="zoom-in" class="text-center">
            <h3 class="text-danger text-center"> Time Out ${player.name}! </h3>
            <div class="text-center">
                <div class="mb-4">
                    <img src="/userTemplate/assets/images/timeOut.png" style="height: 200px; "/>
                </div>
                 <h5>Sorry !</h5>
                 <p class="text-danger">Correct Answer is ${correctAnswer[0].choiceText}</p>
                 <p class="text-muted" >Question Score is ${question.score}  <br/>
                     Total Score ${player.currentTotalScore}
                 </p>    
            </div>
        </div>
`);
    }
    else if (player.isCurrentQuestionCorrect) {
        trueAnswer.play();
        $("#waitingArea").html(`

  <div data-aos="zoom-in" class="text-center">
            <h3 class="text-success text-center"> Correct Answer ${player.name}! </h3>
            <div class="text-center">
                <div class="mb-4">
                    <img src="/userTemplate/assets/images/submit-successfully.png" style="height: 121px; "/>
                </div>
                <h5>Well Done !</h5>
                <p class="text-muted" >         
                     Current Total Score ${player.currentTotalScore}
                </p>
            </div>
        </div>
`); 

      

    } else {
        losing.play();
        console.log(correctAnswer);
        $("#waitingArea").html(`
<div class="text-center" >
          <div data-aos="zoom-in" class="text-center  p-4">
            <h3 class="text-danger"> Wrong Answer  </h3>
            <div class="text-center">
                <div class="mb-4">
                    <img src="/userTemplate/assets/images/submit-wrong.png" style="height: 121px; "/>
                </div>
                <h5>Sorry !</h5>
                 <p class="text-danger">Correct Answer is ${correctAnswer[0].choiceText}</p>
                 <p class="text-muted" >Question Score is ${question.score}  <br/>
                     Total Score ${player.currentTotalScore}
                 </p>      
            </div>
        </div>
</div>`);

    }

    $("#waitingArea").attr("hidden", false);



    $(".form_content").attr("hidden", true);
    $(".countdown_timer").attr("hidden", true);
});


connection.on("GameEnded", function (players) {
    setTimeout(function () {
        gameCompleted.play();
        console.log(players);
        $(".topArea").attr("hidden", true)

        var playersNumber = ""
        var playersData = "";
        var rank = "";

        for (var i = 0; i < players.length; i++) {
            if (i == 0) {
                playersData +=
                    `  

              <li class="list-group-item " style="background-color:rgb(255 32 32 / 12%)!important; padding:1rem !important" >
            <div class="d-flex align-items-center">
                <div class="flex-grow-1">
                    <div class="d-flex">
                        <div class="flex-shrink-0 avatar-xs">
                            <div class="avatar-title bg-soft-danger text-danger rounded">
                               <img src="/userTemplate/assets/images/winner.png" style="height: 40px; "/>
                            </div>
                        </div>  
                        <div class="flex-shrink-0 ms-2">
                            <h6 class="fs-14 mb-0">${players[i].name}</h6>
                        </div>
                    </div>
                </div>
                <div class="flex-shrink-0">
                    <span class="text-danger">${players[i].currentTotalScore}</span>
                </div>
            </div>
        </li>


`
            }
            else if (i > 0 && i < 5) {

                playersData += `  

                <li class="list-group-item " style="padding:1rem !important">
            <div class="d-flex align-items-center">
                <div class="flex-grow-1">
                    <div class="d-flex">
                        <div class="flex-shrink-0 avatar-xs">
                            <div class="avatar-title bg-soft-danger text-danger rounded">
                                                              <img src="/userTemplate/assets/images/badge.png" style="height: 40px; "/>

                            </div>
                        </div>  
                        <div class="flex-shrink-0 ms-2">
                            <h6 class="fs-14 mb-0">${players[i].name}</h6>
                        </div>
                    </div>
                </div>
                <div class="flex-shrink-0">
                    <span class="text-danger">${players[i].currentTotalScore}</span>
                </div>
            </div>
        </li>


             `
            }
            if (connection.connectionId == players[i].connectionId) {
                rank += `<h3 style="font-weight: lighter;">Your Rank is <span style="color:red;">${i + 1}</span></h3>`
            }

        }
        if (players.length > 5) {
            playersNumber = 5;

        } else {
            playersNumber = players.length;

        }
        $('#waitingArea').html(`
<div class="row" style="
    margin-top: 1rem;">
<div class="col-md-4 mt-5">
<div data-simplebar style="max-height: 215px;"> 
    <ul class="list-group">
     <li class="list-group-item " >
                <div class="text-center">
                     <h5 class="p-2"> Top ${playersNumber} Winners !</h5>
                </div>
     </li>

      ${playersData}

    </ul>
</div>
</div>
<div class="col-md-8 text-center mt-5">
<div class="shap_content position-relative">
                    <img  src="/userTemplate/assets/images/see_you.png" alt="image_not_found">
                 
                </div>
<div> ${rank}</div>
</div>
</div>


`);
        $("#waitingArea").attr("hidden", false);



        $(".form_content").attr("hidden", true);
        $(".countdown_timer").attr("hidden", true);

    },1000)
});

//#endregion hub calling

//#region form submit

$("#joinGameForm").on("submit", function (e) {

    e.preventDefault();
    var form = $(this).serialize();


    connection.invoke("JoinPlayersGame", form, gameCode);

});

//#endregion 


//#region functions 

function sendAnswer(selectedAnswer, questionId , responseTime) {


    connection.invoke("SendAnswer", selectedAnswer, gameCode, questionId, responseTime);
}
function getQuestionType(score) {

    if (score == 1000) {

        noramlQues.play();
        $('.questionType').addClass("imageUnvisible");
        $('.questionType').removeClass("imageVisible");

    }
    else if (score == 2000) {


        $('.questionType').html(
            ` <img src="/userTemplate/assets/images/2x.png " style ="width: 100px;" class="animate__animated animate__bounce animate__infinite" />     `
        );

        $('.questionType').addClass("imageVisible");
        $('.questionType').removeClass("imageUnvisible");

        doubleScore.play();


    } else {
        $('.questionType').html(
            ` <img src="/userTemplate/assets/images/3x.png " style ="width: 100px;" class="animate__animated animate__bounce animate__infinite" />     `
        )
        $('.questionType').addClass("imageVisible");
        $('.questionType').removeClass("imageUnvisible");

        tripleScore.play();
    }
}
function checkGameRunning() {
    connection.invoke("CheckGameRunning", gameCode);
    console.log("started");
}
function reEnter() {
    location.reload();

}

//#endregion functions


// #region disconnecting

connection.onclose(error => {
    console.log(error);
    $(".login-form").attr("hidden", true);
    $(".waiting-area").attr("hidden", true);

    disconnected();

    $.blockUI({
        message: `
        <div class="p-5">
            <h3 class="text-danger"> Your're disconnected ! </h3>
            <hr/>
            <p>You are disconnected, please reload the page to connect again with the game  </p>
            <button type="submit" class="btn btn-info mt-3 text-white" onclick="reEnter()">Re-enter The Game</button>
        </div>
        
         <br/>`,
        css: { width: '35rem' }

    });


});


connection.on("DisconnectedGame", function () {

    $(".login-form").attr("hidden", true);
    $(".waiting-area").attr("hidden", true);

    connection.stop();
   disconnected();

    $.blockUI({
        message: `
        <div class="p-5">
            <h3 class="text-danger"> Game Is Disconnected ! </h3>
            <hr/>
            <p> please reload the page if you want to join game again   </p>
            <button type="submit" class="btn btn-info mt-3 text-white" onclick="reEnter()">Re-Join The Game</button>
        </div>
        
         <br/>`,
        css: { width: '35rem' }

    });

});

connection.on("DisconnectedPlayer", function (name) {

    $(`.player_${name.replaceAll(' ', '')}`).remove();
  

});


// #endregion disconnecting

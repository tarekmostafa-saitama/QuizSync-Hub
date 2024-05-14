
/* delete answer */
updateDeleteBtns();

function updateDeleteBtns() {
    var deleteBtns = document.querySelectorAll('.deleteAnsBtn');
    for (const deleteBtn of deleteBtns) {
        deleteBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            const deletedElement = e.target.parentElement;
            deletedElement.remove();

        });
    }
};



var AddAnswerBtn = document.querySelector("#AddAnswer");
var answersContainer = document.querySelector('.answers');

//add new answer btn
AddAnswerBtn.addEventListener('click', () => {
        
        const newAnswer = document.createElement('div');
        newAnswer.classList.add("answer");
        newAnswer.innerHTML = `
                  <input type="text" id="textInput"  class="form-control align-self-center me-2">
                    <input type="radio"  name="IsTrue" class="align-self-center px-3" id="radioInput">
                <button type="button" class="deleteAnsBtn pb-2 pe-2 text-danger">-</button>

`
        answersContainer.append(newAnswer);
        updateDeleteBtns();
  
});




document.querySelector('#setQuestionForm').addEventListener('submit',
    (e) => {
        e.preventDefault();
        var formData = new FormData(document.querySelector('#setQuestionForm'));

        var quesData = setData(formData);

        // Display the key/value pairs
        //for (var pair of formData.entries()) {
        //    console.log(pair[0] + ', ' + pair[1]);
        //}


        if (quesData != false ) {
            ldld.on();
            var url = `/Admin/Games/${formData.get('GameId')}/${formData.get('Id') }/SetQues`

            $.ajax({
                url: url,   
                type: 'POST',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data) {
                   location.reload(true);
                       }

                       else {
                           Snackbar.show({
                               text: 'Something seems Wrong while executing the Add request ',
                               actionTextColor: '#fff',
                               backgroundColor: '#e7515a'

                           });
                       }
                            ldld.off();

                   
                },
                error: function (e) {
               console.log("ERROR : ", e);
               alert('Something seems Wrong while sending the Add request.')
                }
               
            });
        }

    });



function setData(formData) {


     var res= validation(formData)

     if (res == false ) { return false; }
   

    const answersElements = document.querySelectorAll('.answer');

    if (answersElements.length <2) {
        Snackbar.show({
            text: 'Please enter 2 answers at least',
            actionTextColor: '#fff',
            backgroundColor: '#e7515a'

        });
        return false
    }

    for (let i = 0; i < answersElements.length; i++) {


        const answer = answersElements[i].querySelector('#textInput').value;
        const correct = answersElements[i].querySelector('#radioInput').checked;
       

        if (answer == '') {
            Snackbar.show({
                text: 'please fill or delete the empty answer',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a'

            });
            return false
        }

        if (!$("input[name='IsTrue']").is(":checked")) {
            Snackbar.show({
                text: 'Please Choose The Correct Answer',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a'

            });

            return false

        }

      


        formData.append(`Choices[${i}].ChoiceText`, answer);
        formData.append(`Choices[${i}].IsTrue`, correct);

    }
    //for (var pair of formData.entries()) {
    //    console.log(pair[0] + ', ' + pair[1]);
    //}

    var image = formData.get('Photo');
    var imageUrl = $(".custom-file-container__image-preview").css("background-image");
    var bg = imageUrl.replace('url(', '').replace(')', '').replace(/\"/gi, "");
    var imageName = bg.substring(bg.lastIndexOf('/') + 1); 
    if (image != "" && imageName != "none" && image.size == 0) {
        formData.append("PhotoUrl", imageName);

    }
    validation(formData); 
}


function validation(form) {

    if (form.get('Title') == '') {
            Snackbar.show({
                text: 'Please enter the question title',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a'

            });
        return false
    }
    if (form.get('Score')== '') {
           Snackbar.show({
            text: 'Please select the score',
            actionTextColor: '#fff',
            backgroundColor: '#e7515a'

        });
        return false
    }
    if (form.get('Duration') <= 0) {
        Snackbar.show({
            text: 'Please enter the a valid duration in seconds',
            actionTextColor: '#fff',
            backgroundColor: '#e7515a'

        });
        return false
    }

  
}






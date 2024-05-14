// Create Peer Connection
var localStream = ""; 
   
var peer = "";


function createPeerConnection() {
     peer = new Peer({
        key: 'f29622b8-c03e-499d-bc79-2566e8b3c15a',
        debug: 2
    });
    peer.on("open", async () => {

        localStream = await navigator.mediaDevices.getUserMedia({ "audio": true });
        muteAudio(); 

        const sfuRoom = peer.joinRoom("gameCode", {
            mode: "sfu",
            stream: localStream,
        });
        sfuRoom.on("open", () => {
        });
        sfuRoom.on("stream", (stream) => {
            let audio = new Audio();
            audio.srcObject = stream;
            audio.play();
        });
    });
}

function muteAudio() {
    localStream.getAudioTracks().forEach((track) => (track.enabled = false));

}
function unmuteAudio() {
    localStream.getAudioTracks().forEach((track) => (track.enabled = true));
    console.log(localStream);

}


function toggle(isMuted) {
    if (isMuted == true) {
          //muteed

        $(".micIcon").html(`
          
          <button id="Mic-Icon" onclick="toggle(false)"><i class="fas fa-microphone-slash"></i></button>
`
        )
        muteAudio(); 
          //unmuteed

    } else {
        $(".micIcon").html(`
          <button id="Mic-Icon" onclick="toggle(true)"><i class="fas fa-microphone-alt"></button>`
        )

        unmuteAudio();  
    }
}

function disconnected() {
    console.log(peer);
    peer.destroy();
}

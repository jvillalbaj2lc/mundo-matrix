using UnityEngine;
using UnityEngine.UI;

namespace Afrodita.is4You2
{
    public class Launcher : Photon.PunBehaviour
    {
        #region Public Variables
        /// <summary>
        /// The PUN loglevel. 
        /// </summary>
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
        [Tooltip("Consola estandar para mostrar las acciones del código por pantalla.")]
        public InputField controlInputVisual;
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>   
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte MaxPlayersPerRoom = 4;
        
        /// <summary>
        /// Indica la escena que se va a cargar.
        /// </summary>
        public enum Escenas {
            Camerino
        }
        #endregion

        #region Private Variables
        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
        /// </summary>
        string _gameVersion = "1";
        bool isConnecting;
        const string MENSAJE_CONECTANDO_SERVIDOR = "1. Conectando con el servidor, por favor espere...\n";
        const string MENSAJE_CONEXION_YA_ESTABLECIDA = "2. La aplicación ya estaba conectada al servidor de Photon Cloud...\n";
        const string MENSAJE_CONEXION_SERVIDOR_CONFIGURACION = "2.2 Se ha conectado correctamente la conexión mediante configuración al servidor Photon Cloud...\n";
        const string MENSAJE_CONEXION_SERVIDOR_MAESTRO = "3. Se ha realizado la conexión al Servidor Maestro...\n";
        const string MENSAJE_CONEXION_SALA = "4. El jugador ha entrado en una sala del servidor...\n";
        const string MENSAJE_CONECTAR_SALA_ALEATORIA = "5. El jugador se va a conectar a una sala aleatoria...\n";
        const string MENSAJE_CARGAR_ESCENA = "6. El jugador va a entrar a la escena de:";
        const string MENSAJE_DESCONEXION_SERVIDOR = "99. Se ha desconectado del servidor de Photon Cloud...\n";
        #endregion

        #region MonoBehaviour CallBacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake() {

            PhotonNetwork.logLevel = Loglevel;
            // #Critical
            // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;


            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;
        }
        #endregion

        void Start() {
        }

        #region Public Methods
        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            controlInputVisual.text = controlInputVisual.text + MENSAJE_CONECTANDO_SERVIDOR;

            // Comprobamos si está abierta previamente una conexión, si no abrimos una nueva.
            if (PhotonNetwork.connected) {
                controlInputVisual.text = controlInputVisual.text + MENSAJE_CONEXION_YA_ESTABLECIDA;
                // #Critical Si ya está abierta la conexión, entramos a un Room de lo contrario se lanzará la Excepción OnPhotonRandomJoinFailed() y se creará uno.
                PhotonNetwork.JoinRandomRoom();
            }
            else {
                controlInputVisual.text = controlInputVisual.text + MENSAJE_CONEXION_SERVIDOR_CONFIGURACION;
                // Primero y más importante debemos realizar una conexión al servidor de Photon.
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }
        #endregion

        #region Photon.PunBehaviour CallBacks
        public override void OnConnectedToMaster() {

                controlInputVisual.text = controlInputVisual.text + MENSAJE_CONEXION_SERVIDOR_MAESTRO;
                // 01. Al conectarse al Servidor Maestro, ya se puede realizar la conexión al Room.
                PhotonNetwork.JoinRandomRoom();
        }
        public override void OnDisconnectedFromPhoton() {
            controlInputVisual.text = controlInputVisual.text + MENSAJE_DESCONEXION_SERVIDOR;
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {

            controlInputVisual.text = controlInputVisual.text + MENSAJE_CONECTAR_SALA_ALEATORIA;
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }
        public override void OnJoinedRoom() {

            // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.room.playerCount == 1){
                controlInputVisual.text = controlInputVisual.text + MENSAJE_CONEXION_SALA;
                PhotonNetwork.LoadLevel(Escenas.Camerino.ToString());
            }
            controlInputVisual.text = controlInputVisual.text + MENSAJE_CARGAR_ESCENA + Escenas.Camerino.ToString() + "...\n";
        }
        #endregion
    }
}

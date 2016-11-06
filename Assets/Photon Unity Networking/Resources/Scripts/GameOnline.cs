using UnityEngine;
using UnityEngine.UI;

namespace Afrodita.is4You2 { 
    public class GameOnline : Photon.PunBehaviour {

        #region Variables Públicas.
        static public GameOnline Instance;
        [Tooltip("El prefab que va a representar al jugador en el servidor.")]
        public GameObject playerPrefab;

        [Tooltip("Consola estandar para mostrar las acciones del código por pantalla.")]
        public InputField controlInputVisual;
        #endregion

        #region Variables Privadas.
        const string MENSAJE_INICIALIZANDO_ESCENA = "1. Se ha cargado la escena en el servidor...\n";
        const string MENSAJE_MODO_OFFLINE = "1.1 No está conectado al servidor...\n";
        const string MENSAJE_JUGADOR_CONECTADO = "2. Se ha conectado el jugador:";
        const string MENSAJE_JUGADOR_INSTANCIADO = "3. Se ha instanciado correctamente el jugador...\n";
        const string MENSAJE_JUGADOR_DESCONECTADO = "4. Se ha desconectado el jugador:";
        const string MENSAJE_ERROR_CONEXION_SERVIDOR = "99. Error al conectar al servidor, por favor revisar el log.\n";
        #endregion
        void Start () {
            if (PhotonNetwork.connected) {
                controlInputVisual.text = controlInputVisual.text + MENSAJE_INICIALIZANDO_ESCENA;
                GameObject jugadorCargado = PhotonNetwork.Instantiate("ibu", new Vector3(0, 5, 0), Quaternion.identity, 0);
                jugadorCargado.SetActive(true);
            }
            else
            {
                Instantiate(playerPrefab);
                controlInputVisual.text = controlInputVisual.text + MENSAJE_MODO_OFFLINE;
            }
        }

        #region Métodos de Photon Cloud.
        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            controlInputVisual.text = controlInputVisual.text + MENSAJE_JUGADOR_CONECTADO + other.name + "\n";
            if (PhotonNetwork.isMasterClient){
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
            }
        }
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            controlInputVisual.text = controlInputVisual.text = MENSAJE_JUGADOR_INSTANCIADO;
        }
        public override void OnConnectionFail(DisconnectCause cause)
        {
            controlInputVisual.text = controlInputVisual.text = MENSAJE_ERROR_CONEXION_SERVIDOR;
            controlInputVisual.text = controlInputVisual.text = cause.ToString();
        }
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            controlInputVisual.text = controlInputVisual.text + MENSAJE_JUGADOR_DESCONECTADO + otherPlayer.name + "\n";
        }
        #endregion
    }
}
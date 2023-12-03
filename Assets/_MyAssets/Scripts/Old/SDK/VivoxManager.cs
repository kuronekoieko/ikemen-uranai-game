using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Services.Vivox;
using VivoxUnity;
using System;
using System.ComponentModel;


/// <summary>
/// https://docs.vivox.com/v5/general/unity/15_1_190000/ja-jp/Default.htm#Unity/developer-guide/vivox-unity-sdk-basics/initialize-voice-client-object.htm?TocPath=Vivox%2520Unity%2520SDK%2520%25E3%2581%25AE%25E3%2583%2589%25E3%2582%25AD%25E3%2583%25A5%25E3%2583%25A1%25E3%2583%25B3%25E3%2583%2588%257CVivox%2520Unity%2520%25E9%2596%258B%25E7%2599%25BA%25E8%2580%2585%25E3%2582%25AC%25E3%2582%25A4%25E3%2583%2589%257CClient%2520SDK%2520%25E3%2581%25AE%25E5%259F%25BA%25E6%259C%25AC%257C_____1
/// </summary>
public class VivoxManager : MonoBehaviour
{
    Client _client;
    Uri _serverUri = new Uri("https://unity.vivox.com/appconfig/15668-___-35308-udash");
    string tokenSigningKey = "y3kqlSDvgSF6M66C8i4Mr1E3c9OMKg7o";
    string tokenIssuer = "15668-___-35308-udash";
    TimeSpan tokenExpirationDuration = new TimeSpan(0, 1, 0);
    ILoginSession _loginSession;
    Account account;
    IChannelSession channelSession;

    public void Initialize()
    {


    }


    private async void Awake()
    {
        _client = new Client();
        _client.Uninitialize();
        _client.Initialize();

        // authのユーザーidは端末で決まるので、別端末でテストする必要あり
        await UnityAuthenticationManager.Instance.Initialize();
        VivoxService.Instance.Initialize();
        await LoginUser();
        await JoinChannel("channel-001");
        BindChannelSessionHandlers(true, channelSession);
    }

    IEnumerator LoginUser()
    {
        // この例では、クライアントが初期化されています。
        account = new Account();
        _loginSession = _client.GetLoginSession(account);

        IAsyncResult result = _loginSession.BeginLogin(_serverUri, _loginSession.GetLoginToken(tokenSigningKey, tokenExpirationDuration), ar =>
        {
            try
            {
                _loginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                // エラー処理
                throw e;
            }
            // この時点でログインは成功しており、他の操作を実行できます。

        });

        while (result.IsCompleted == false)
        {
            yield return null;
        }

        _loginSession.PropertyChanged += onLoginSessionPropertyChanged;
    }

    private void onLoginSessionPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        if ("State" == propertyChangedEventArgs.PropertyName)
        {
            switch ((sender as ILoginSession).State)
            {
                case LoginState.LoggingIn:
                    // 必要に応じて操作 
                    break;

                case LoginState.LoggedIn:
                    // 必要に応じて操作 
                    break;

                case LoginState.LoggedOut:
                    // 必要に応じて操作 
                    break;
                default:
                    break;
            }
        }
    }

    void LogOut()
    {
        // この例では、_loginSession が ILoginSession になると見なされます。
        _loginSession.Logout();
    }

    void OnApplicationQuit()
    {
        _loginSession.Logout();
        _client.Uninitialize();
    }

    IEnumerator JoinChannel(string channelName)
    {
        var channel = new Unity.Services.Vivox.Channel(channelName, ChannelType.NonPositional);
        channelSession = _loginSession.GetChannelSession(channel);

        // すべてのチャンネルのプロパティの変更にサブスクライブする。
        channelSession.PropertyChanged += SourceOnChannelPropertyChanged;

        // チャンネルに接続する
        IAsyncResult result = channelSession.BeginConnect(true, true, true, channelSession.GetConnectToken(tokenSigningKey, tokenExpirationDuration), ar =>
        {
            try
            {
                channelSession.EndConnect(ar);
            }
            catch (Exception e)
            {
                // エラー処理
                throw e;
            }

            // この点に到達することはエラーが発生していないことを示しますが、AudioState か TextState（またはその両方）が ConnectionState.Connected になるまでユーザーが「チャンネルにいる」ことにはなりません。
        });

        while (result.IsCompleted == false)
        {
            yield return null;
        }
    }

    void SourceOnChannelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        var channelSession = (IChannelSession)sender;

        // この例では、AudioState の変更のみをチェックしています。
        if (propertyChangedEventArgs.PropertyName == "AudioState")
        {
            switch (channelSession.AudioState)
            {
                case ConnectionState.Connecting:
                    Debug.Log("Audio connecting in " + channelSession.Key.Name);
                    break;

                case ConnectionState.Connected:
                    Debug.Log("Audio connected in " + channelSession.Key.Name);
                    break;

                case ConnectionState.Disconnecting:
                    Debug.Log("Audio disconnecting in " + channelSession.Key.Name);
                    break;

                case ConnectionState.Disconnected:
                    Debug.Log("Audio disconnected in " + channelSession.Key.Name);
                    break;
            }
        }
    }

    void LeaveChannel(ChannelId channelIdToLeave)
    {

        var channelSession = _loginSession.GetChannelSession(channelIdToLeave);
        if (channelSession != null)
        {
            // チャンネルから切断
            channelSession.Disconnect();
            //（必要に応じて）チャンネルリストからチャンネルエントリを削除
            _loginSession.DeleteChannelSession(channelIdToLeave);
        }

    }

    private void BindChannelSessionHandlers(bool doBind, IChannelSession channelSession)
    {
        //イベントへのサブスクライブ
        if (doBind)
        {
            // 参加者
            channelSession.Participants.AfterKeyAdded += OnParticipantAdded;
            channelSession.Participants.BeforeKeyRemoved += OnParticipantRemoved;
            channelSession.Participants.AfterValueUpdated += OnParticipantValueUpdated;

            //メッセージ
            // channelSession.MessageLog.AfterItemAdded += OnChannelMessageReceived;
        }

        //イベントのサブスクライブ解除
        else
        {
            // 参加者
            channelSession.Participants.AfterKeyAdded -= OnParticipantAdded;
            channelSession.Participants.BeforeKeyRemoved -= OnParticipantRemoved;
            channelSession.Participants.AfterValueUpdated -= OnParticipantValueUpdated;

            //メッセージ
            // channelSession.MessageLog.AfterItemAdded -= OnChannelMessageReceived;
        }
    }


    private void OnParticipantAdded(object sender, KeyEventArg<string> keyEventArg)
    {
        ValidateArgs(new object[] { sender, keyEventArg });

        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        var participant = source[keyEventArg.Key];
        var username = participant.Account.Name;
        var channel = participant.ParentChannelSession.Key;
        var channelSession = participant.ParentChannelSession;
        //この情報を使用して必要な処理を実行
        if (participant.IsSelf) Debug.Log("自分 が参加しました " + username);
        if (!participant.IsSelf) Debug.Log("相手 が参加しました " + username);

    }

    private static void ValidateArgs(object[] objs)
    {
        foreach (var obj in objs)
        {
            if (obj == null)
                throw new ArgumentNullException(obj.GetType().ToString(), "Specify a non-null/non-empty argument.");
        }
    }


    private void OnParticipantRemoved(object sender, KeyEventArg<string> keyEventArg)
    {
        ValidateArgs(new object[] { sender, keyEventArg });

        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        var participant = source[keyEventArg.Key];
        var username = participant.Account.Name;
        var channel = participant.ParentChannelSession.Key;
        var channelSession = participant.ParentChannelSession;
        // uIManager.DeleteUserMuteObjectUI(username);
        if (participant.IsSelf) Debug.Log("自分 が退室しました " + username);
        if (!participant.IsSelf) Debug.Log("相手 が退室しました " + username);

        if (participant.IsSelf)
        {
            BindChannelSessionHandlers(false, channelSession); //ここでイベントからサブスクライブ解除

            // currentChannelID = null;

            var user = _client.GetLoginSession(account);
            user.DeleteChannelSession(channelSession.Channel);
        }
    }

    private void OnParticipantValueUpdated(object sender, ValueEventArg<string, IParticipant> valueEventArg)
    {
        ValidateArgs(new object[] { sender, valueEventArg }); //ここまでの post のコードを参照

        var source = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;
        var participant = source[valueEventArg.Key];

        string username = valueEventArg.Value.Account.Name;
        ChannelId channel = valueEventArg.Value.ParentChannelSession.Key;
        string property = valueEventArg.PropertyName;

        switch (property)
        {
            case "LocalMute":
                {
                    if (username != account.Name) //自分をローカルミュートすることはできないため、チェックしないでください
                    {
                        //ミュートされた画像を更新
                    }
                    break;
                }
            case "SpeechDetected":
                {
                    ///発話インジケーターの画像を更新
                    break;
                }
            default:
                break;
        }
    }


}


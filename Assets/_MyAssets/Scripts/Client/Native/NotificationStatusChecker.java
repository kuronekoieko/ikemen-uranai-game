package com.ikevo.studio.notification;
import com.unity3d.player.UnityPlayer;
import android.content.Context;
import androidx.core.app.NotificationManagerCompat;
/* NotificationStatusChecker
    Androidのプッシュ通知許諾設定を取得するクラス
*/ 
public class NotificationStatusChecker{
 
    private Context context;
 
    public NotificationStatusChecker() {
 
        this.context = UnityPlayer.currentActivity;
    }
 
    public boolean areNotificationsEnabled() {
        boolean result = NotificationManagerCompat.from(this.context).areNotificationsEnabled();
        return result;
    }
}
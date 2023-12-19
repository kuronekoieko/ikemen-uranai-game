using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

/// <summary>
/// 期間を保持するクラス
/// https://qiita.com/M_Kagawa/items/7ce2090ff9163d2fd73f
/// </summary>
[DebuggerDisplay("{TruncatedSince.ToString(\"MM/dd HH:mm:ss\")} ～ {TruncatedUntil.ToString(\"MM/dd HH:mm:ss\")} [ {Level} ]")]
public class DateTimePeriod
{
    /// <summary>開始日時</summary>
    public DateTime Since { get; }

    /// <summary>終了日時</summary>
    public DateTime Until { get; }

    /// <summary>切り捨て粒度</summary>
    public TruncateLevel Level { get; }

    /// <summary>開始日時(切り捨て)</summary>
    public DateTime TruncatedSince { get => Truncate(Since, Level); }

    /// <summary>終了日時(切り捨て)</summary>
    public DateTime TruncatedUntil { get => Truncate(Until, Level); }

    /// <summary>期間(年)</summary>
    public double Years { get => (TruncatedUntil - TruncatedSince).TotalDays / 365; }

    /// <summary>期間(月)</summary>
    public double Monthes { get => (TruncatedUntil - TruncatedSince).TotalDays / 12; }

    /// <summary>期間(日)</summary>
    public double Days { get => (TruncatedUntil - TruncatedSince).TotalDays; }

    /// <summary>期間(時間)</summary>
    public double Hours { get => (TruncatedUntil - TruncatedSince).TotalHours; }

    /// <summary>期間(分)</summary>
    public double Minutes { get => (TruncatedUntil - TruncatedSince).TotalMinutes; }

    /// <summary>期間(秒)</summary>
    public double Seconds { get => (TruncatedUntil - TruncatedSince).TotalSeconds; }

    /// <summary>期間(ミリ秒)</summary>
    public double MilliSeconds { get => (TruncatedUntil - TruncatedSince).TotalMilliseconds; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sinceDateTime">開始日時</param>
    /// <param name="untilDateTime">終了日時</param>
    /// <param name="truncateLevel">切り捨て粒度</param>
    public DateTimePeriod(DateTime sinceDateTime, DateTime untilDateTime, TruncateLevel truncateLevel = TruncateLevel.Full)
    {
        if (sinceDateTime > untilDateTime) throw new ArgumentException("無効な期間です");

        Since = sinceDateTime;
        Until = untilDateTime;
        Level = truncateLevel;
    }

    /// <summary>
    /// 対象日時が期間に含まれているかを判定
    /// </summary>
    /// <param name="targetDateTime">比較対象の日時</param>
    /// <param name="allowSameTime">同時刻でもtrueとするか</param>
    /// <returns></returns>
    public bool Contains(DateTime targetDateTime, bool allowSameTime = true)
    {
        if (targetDateTime == default || targetDateTime == DateTime.MinValue || targetDateTime == DateTime.MaxValue) return false;

        bool result = false;

        // 期間と比較対象日時を同じ粒度で切り捨ててから比較
        DateTime truncatedTarget = Truncate(targetDateTime, Level);

        if ((allowSameTime && TruncatedSince <= truncatedTarget && truncatedTarget <= TruncatedUntil)
            || (!allowSameTime && TruncatedSince < truncatedTarget && truncatedTarget < TruncatedUntil))
        {
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 対象期間と交差するかを判定
    /// </summary>
    /// <param name="targetPeriod"></param>
    /// <returns></returns>
    public bool Intersects(DateTimePeriod targetPeriod)
    {
        return Intersects(this, targetPeriod);
    }

    /// <summary>
    /// 期間同士が交差or内包するかを判定
    /// </summary>
    /// <param name="period1">比較する期間1</param>
    /// <param name="period2">比較する期間2</param>
    /// <returns></returns>
    private static bool Intersects(DateTimePeriod period1, DateTimePeriod period2)
    {
        // 交差も内包もしないのは2パターンしかない
        /*
        * |---Period1---|
        *                 |---Period2---|
        * もしくは
        *                 |---Period1---|
        * |---Period2---|
        */

        return !(period1.TruncatedUntil < period2.TruncatedSince || period2.TruncatedUntil < period1.TruncatedSince);
    }

    /// <summary>
    /// 時刻の切り捨て処理
    /// 比較を正規化するためDateTime内部のTicksを切り捨てる
    /// </summary>
    /// <param name="targetDateTime">切り捨て対象の日時</param>
    /// <param name="truncateLevel">切り捨て粒度</param>
    /// <returns></returns>
    private static DateTime Truncate(DateTime targetDateTime, TruncateLevel truncateLevel)
    {
        if (targetDateTime == DateTime.MinValue || targetDateTime == DateTime.MaxValue || truncateLevel == TruncateLevel.Full) return targetDateTime;

        TimeSpan timeSpan = default;

        switch (truncateLevel)
        {
            case TruncateLevel.Day: timeSpan = TimeSpan.FromHours(1); break;
            case TruncateLevel.Hour: timeSpan = TimeSpan.FromMinutes(1); break;
            case TruncateLevel.Minute: timeSpan = TimeSpan.FromSeconds(1); break;
            case TruncateLevel.Second: timeSpan = TimeSpan.FromMilliseconds(1); break;
            case TruncateLevel.MilliSecond: timeSpan = TimeSpan.FromMilliseconds(1); break;
            default: break;
        }

        return targetDateTime.AddTicks(-(targetDateTime.Ticks % timeSpan.Ticks));
    }
}

public enum TruncateLevel { Full, Day, Hour, Minute, Second, MilliSecond }

namespace Blazor.Ink.Test.Helper;

public static class TaskHelper
{
  /// <summary>
  /// 指定した条件がtrueになるまで待機します。
  /// </summary>
  /// <param name="condition">待機する条件</param>
  /// <param name="pollingIntervalMilliseconds">ポーリング間隔（ミリ秒）</param>
  /// <param name="cancellationToken">キャンセル用トークン</param>
  /// <exception cref="OperationCanceledException">キャンセル時にスローされます</exception>
  public static async Task WaitUntil(Func<bool> condition, int pollingIntervalMilliseconds = 50, CancellationToken cancellationToken = default)
  {
    while (!condition())
    {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Delay(pollingIntervalMilliseconds, cancellationToken);
    }
  }

  /// <summary>
  /// 指定した条件がtrueになるまで待機します。
  /// ポーリング間隔は50ミリ秒に設定されます。
  /// </summary>
  /// <param name="condition">待機する条件</param>
  /// <param name="cancellationToken">キャンセル用トークン</param>
  /// <exception cref="OperationCanceledException">キャンセル時にスローされます</exception>
  public static Task WaitUntil(Func<bool> condition, CancellationToken cancellationToken)
  {
    return WaitUntil(condition, 50, cancellationToken);
  }
}
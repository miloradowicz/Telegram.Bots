// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright © 2020 Aman Agnihotri

using System;
using System.Collections.Generic;
using Telegram.Bots.Types;

namespace Telegram.Bots.Requests
{
  using Options = IEnumerable<string>;

  public abstract record SendPoll<TChatId> : IRequest<PollMessage>,
    IChatTargetable<TChatId>, INotifiable, IReplyable, IMarkupable
  {
    public TChatId ChatId { get; }

    public string Question { get; }

    public Options Options { get; }

    public bool? IsAnonymous { get; init; }

    public abstract PollType Type { get; }

    public int? OpenPeriod { get; }

    public DateTime? CloseDate { get; }

    public bool? IsClosed { get; init; }

    public bool? DisableNotification { get; init; }

    public int? ReplyToMessageId { get; init; }

    public bool? AllowSendingWithoutReply { get; init; }

    public ReplyMarkup? ReplyMarkup { get; init; }

    public string Method { get; } = "sendPoll";

    protected SendPoll(TChatId chatId, string question, Options options, int openPeriod)
    {
      ChatId = chatId;
      Question = question;
      Options = options;
      OpenPeriod = openPeriod;
    }

    protected SendPoll(TChatId chatId, string question, Options options, DateTime? closeDate)
    {
      ChatId = chatId;
      Question = question;
      Options = options;
      CloseDate = closeDate;
    }
  }

  public abstract record SendRegularPoll<TChatId> : SendPoll<TChatId>
  {
    public override PollType Type { get; } = PollType.Regular;

    public bool? AllowsMultipleAnswers { get; init; }

    protected SendRegularPoll(TChatId chatId, string question, Options options, int openPeriod) :
      base(chatId, question, options, openPeriod) { }

    protected SendRegularPoll(
      TChatId chatId,
      string question,
      Options options,
      DateTime? closeDate) :
      base(chatId, question, options, closeDate) { }
  }

  public abstract record SendQuizPoll<TChatId> : SendPoll<TChatId>
  {
    public override PollType Type { get; } = PollType.Quiz;

    public uint CorrectOptionId { get; init; }

    public string? Explanation { get; init; } = null!;

    public ParseMode? ExplanationParseMode { get; init; }

    public IEnumerable<MessageEntity>? ExplanationEntities { get; init; }

    protected SendQuizPoll(
      TChatId chatId,
      string question,
      Options options,
      uint correctOptionId,
      int openPeriod) :
      base(chatId, question, options, openPeriod) => CorrectOptionId = correctOptionId;

    protected SendQuizPoll(
      TChatId chatId,
      string question,
      Options options,
      uint correctOptionId,
      DateTime? closeDate) :
      base(chatId, question, options, closeDate) => CorrectOptionId = correctOptionId;
  }

  public sealed record SendRegularPoll : SendRegularPoll<long>
  {
    public SendRegularPoll(long chatId, string question, Options options, int openPeriod) :
      base(chatId, question, options, openPeriod) { }

    public SendRegularPoll(long chatId, string question, Options options, DateTime? closeDate) :
      base(chatId, question, options, closeDate) { }
  }

  public sealed record SendQuizPoll : SendQuizPoll<long>
  {
    public SendQuizPoll(
      long chatId,
      string question,
      Options options,
      uint correctOptionId,
      int openPeriod) : base(chatId, question, options, correctOptionId, openPeriod) { }

    public SendQuizPoll(
      long chatId,
      string question,
      Options options,
      uint correctOptionId,
      DateTime? closeDate) : base(chatId, question, options, correctOptionId, closeDate) { }
  }

  namespace Usernames
  {
    public sealed record SendRegularPoll : SendRegularPoll<string>
    {
      public SendRegularPoll(string username, string question, Options options, int openPeriod) :
        base(username, question, options, openPeriod) { }

      public SendRegularPoll(
        string username,
        string question,
        Options options,
        DateTime? closeDate) :
        base(username, question, options, closeDate) { }
    }

    public sealed record SendQuizPoll : SendQuizPoll<string>
    {
      public SendQuizPoll(
        string username,
        string question,
        Options options,
        uint correctOptionId,
        int openPeriod) : base(username, question, options, correctOptionId, openPeriod) { }

      public SendQuizPoll(
        string username,
        string question,
        Options options,
        uint correctOptionId,
        DateTime? closeDate) : base(username, question, options, correctOptionId, closeDate) { }
    }
  }
}

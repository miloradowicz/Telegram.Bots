// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright © 2020-2022 Aman Agnihotri

using GetInlineGameHighScores =
  Telegram.Bots.Requests.Games.Inline.GetGameHighScores;
using SetInlineGameScore = Telegram.Bots.Requests.Games.Inline.SetGameScore;
using static Telegram.Bots.Types.Inline.DocumentMimeType;
using static Telegram.Bots.Types.Inline.VideoMimeType;

namespace Telegram.Bots.Tests.Units.Json;

using Bots.Json;
using Bots.Types;
using Bots.Types.Games;
using Bots.Types.Inline;
using Bots.Types.Passport;
using Bots.Types.Payments;
using Bots.Types.Stickers;
using Requests;
using Requests.Games;
using Requests.Inline;
using Requests.Payments;
using Requests.Stickers;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using static System.StringComparison;
using EditCaption = Requests.Inline.EditCaption;
using EditLiveLocation = Requests.Inline.EditLiveLocation;
using EditMediaViaCache = Requests.Inline.EditMediaViaCache;
using EditMediaViaUrl = Requests.Inline.EditMediaViaUrl;
using EditReplyMarkup = Requests.Inline.EditReplyMarkup;
using EditText = Requests.Inline.EditText;
using FileInfo = Bots.Types.FileInfo;
using StopLiveLocation = Requests.Inline.StopLiveLocation;

public sealed class ContractResolverTests : IClassFixture<Serializer>
{
  private static readonly InputFile File = Stream.Null;
  private static readonly Uri Uri = new("https://example.com");

  private readonly Serializer _serializer;

  public ContractResolverTests(Serializer serializer)
  {
    _serializer = serializer;
  }

  public static TheoryData<(string, object)> SerializationData => new()
  {
    (@"""file_name"":""", new Animation
    {
      Name = ""
    }),
    (@"""game_short_name"":""", new GameCallbackQuery
    {
      ShortName = ""
    }),
    (@"""inline_message_id"":""", new InlineMessageCallbackQuery
    {
      MessageId = ""
    }),
    (@"""inline_message_id"":""", new InlineGameCallbackQuery
    {
      MessageId = ""
    }),
    (@"""game_short_name"":""", new InlineGameCallbackQuery
    {
      ShortName = ""
    }),
    (@"""pinned_message"":{", new ChatInfo
    {
      Pinned = new TextMessage
      {
        Text = ""
      }
    }),
    (@"""file_name"":""", new Document
    {
      Name = ""
    }),
    (@"""file_name"":""", new Audio
    {
      Name = ""
    }),
    (@"""file_name"":""", new Video
    {
      Name = ""
    }),
    (@"""file_id"":""", new Audio
    {
      Id = ""
    }),
    (@"""file_unique_id"":""", new Photo
    {
      UniqueId = ""
    }),
    (@"""file_size"":1", new Video
    {
      Size = 1
    }),
    (@"""file_path"":""", new FileInfo
    {
      Path = ""
    }),
    (@"""resize_keyboard"":true", new KeyboardMarkup(null!)
    {
      Resize = true
    }),
    (@"""one_time_keyboard"":true", new KeyboardMarkup(null!)
    {
      OneTime = true
    }),
    (@"""inline_keyboard"":[",
      new InlineKeyboardMarkup(new List<InlineButton[]>())),
    (@"""callback_data"":""", new CallbackDataButton("", "")),
    (@"""switch_inline_query"":""", new SwitchInlineQueryButton("", "")),
    (@"""switch_inline_query_current_chat"":""",
      new SwitchInlineQueryCurrentChatButton("", "")),
    (@"""callback_game"":{", new CallbackGameButton("")),
    (@"""photo"":[", new Game
    {
      PhotoSet = new List<Photo>()
    }),
    (@"""inline_message_id"":""", new ChosenInlineResult
    {
      MessageId = ""
    }),
    (@"""input_message_content"":{", new InlineArticle("", "")
    {
      Content = new TextContent("")
    }),
    (@"""thumb_url"":""", new InlineArticle("", "")
    {
      Thumb = Uri
    }),
    (@"""audio_file_id"":""", new InlineCachedAudio("", "")),
    (@"""audio_url"":""", new InlineAudio("", "", Uri)),
    (@"""audio_duration"":10", new InlineAudio("", "", Uri)
    {
      Duration = 10
    }),
    (@"""thumb_url"":""", new InlineContact("", "", "")
    {
      Thumb = Uri
    }),
    (@"""document_file_id"":""", new InlineCachedDocument("", "", "")),
    (@"""document_url"":""", new InlineDocument("", "", Uri, Pdf)),
    (@"""thumb_url"":""", new InlineDocument("", "", Uri, Zip)
    {
      Thumb = Uri
    }),
    (@"""game_short_name"":""", new InlineGame("", "")),
    (@"""gif_file_id"":""", new InlineCachedGif("", "")),
    (@"""gif_url"":""", new InlineGif("", Uri, Uri)),
    (@"""gif_width"":1", new InlineGif("", Uri, Uri)
    {
      Width = 1
    }),
    (@"""gif_height"":1", new InlineGif("", Uri, Uri)
    {
      Height = 1
    }),
    (@"""gif_duration"":1", new InlineGif("", Uri, Uri)
    {
      Duration = 1
    }),
    (@"""thumb_url"":""", new InlineGif("", Uri, Uri)),
    (@"""thumb_url"":""", new InlineLocation("", "", 1, 1)
    {
      Thumb = Uri
    }),
    (@"""mpeg4_file_id"":""", new InlineCachedMpeg4Gif("", "")),
    (@"""mpeg4_url"":""", new InlineMpeg4Gif("", Uri, Uri)),
    (@"""mpeg4_width"":1", new InlineMpeg4Gif("", Uri, Uri)
    {
      Width = 1
    }),
    (@"""mpeg4_height"":1", new InlineMpeg4Gif("", Uri, Uri)
    {
      Height = 1
    }),
    (@"""mpeg4_duration"":1", new InlineMpeg4Gif("", Uri, Uri)
    {
      Duration = 1
    }),
    (@"""thumb_url"":""", new InlineMpeg4Gif("", Uri, Uri)),
    (@"""photo_file_id"":""", new InlineCachedPhoto("", "")),
    (@"""photo_url"":""", new InlinePhoto("", Uri, Uri)),
    (@"""thumb_url"":""", new InlinePhoto("", Uri, Uri)),
    (@"""photo_width"":1", new InlinePhoto("", Uri, Uri)
    {
      Width = 1
    }),
    (@"""photo_height"":1", new InlinePhoto("", Uri, Uri)
    {
      Height = 1
    }),
    (@"""sticker_file_id"":""", new InlineCachedSticker("", "")),
    (@"""thumb_url"":""", new InlineVenue("", "", "", 1, 1)
    {
      Thumb = Uri
    }),
    (@"""video_file_id"":""", new InlineCachedVideo("", "", "")),
    (@"""video_url"":""", new InlineVideo("", "", Uri, Html, Uri)),
    (@"""thumb_url"":""", new InlineVideo("", "", Uri, Html, Uri)),
    (@"""video_width"":1", new InlineVideo("", "", Uri, Html, Uri)
    {
      Width = 1
    }),
    (@"""video_height"":1", new InlineVideo("", "", Uri, Html, Uri)
    {
      Height = 1
    }),
    (@"""video_duration"":1", new InlineVideo("", "", Uri, Html, Uri)
    {
      Duration = 1
    }),
    (@"""voice_file_id"":""", new InlineCachedVoice("", "", "")),
    (@"""voice_url"":""", new InlineVoice("", "", Uri)),
    (@"""voice_duration"":10", new InlineVoice("", "", Uri)
    {
      Duration = 10
    }),
    (@"""inline_message_id"":""", new SentWebAppMessage
    {
      MessageId = ""
    }),
    (@"""message_text"":""", new TextContent("")),
    (@"""message_id"":1", new MessageId(1)),
    (@"""message_id"":1", new TextMessage
    {
      Id = 1
    }),
    (@"""message_thread_id"":1", new TextMessage
    {
      ThreadId = 1
    }),
    (@"""animation"":{", new AnimationMessage
    {
      Animation = new Animation()
    }),
    (@"""venue"":{", new VenueMessage
    {
      Venue = new Venue()
    }),
    (@"""text"":""Test""", new TextMessage
    {
      Text = "Test"
    }),
    (@"""audio"":{", new AudioMessage
    {
      Audio = new Audio()
    }),
    (@"""document"":{", new DocumentMessage
    {
      Document = new Document()
    }),
    (@"""photo"":[", new PhotoMessage
    {
      PhotoSet = new List<Photo>()
    }),
    (@"""sticker"":{", new StickerMessage
    {
      Sticker = new Sticker()
    }),
    (@"""video"":{", new VideoMessage
    {
      Video = new Video()
    }),
    (@"""video_note"":{", new VideoNoteMessage
    {
      VideoNote = new VideoNote()
    }),
    (@"""voice"":{", new VoiceMessage
    {
      Voice = new Voice()
    }),
    (@"""contact"":{", new ContactMessage
    {
      Contact = new Contact()
    }),
    (@"""dice"":{", new DiceMessage
    {
      Dice = new Dice()
    }),
    (@"""game"":{", new GameMessage
    {
      Game = new Game()
    }),
    (@"""poll"":{", new PollMessage
    {
      Poll = new RegularPoll()
    }),
    (@"""location"":{", new LocationMessage
    {
      Location = new Location(0, 0)
    }),
    (@"""new_chat_members"":[", new NewChatMembersMessage
    {
      Members = new List<User>()
    }),
    (@"""left_chat_member"":{", new LeftChatMemberMessage
    {
      Member = new User()
    }),
    (@"""new_chat_title"":""""", new NewChatTitleMessage
    {
      Title = ""
    }),
    (@"""new_chat_photo"":[", new NewChatPhotoMessage
    {
      PhotoSet = new List<Photo>()
    }),
    (@"""delete_chat_photo"":true", new DeleteChatPhotoMessage
    {
      Deleted = true
    }),
    (@"""group_chat_created"":true", new GroupChatCreatedMessage
    {
      Created = true
    }),
    (@"""supergroup_chat_created"":true", new SupergroupChatCreatedMessage
    {
      Created = true
    }),
    (@"""channel_chat_created"":true", new ChannelChatCreatedMessage
    {
      Created = true
    }),
    (@"""message_auto_delete_time"":1", new AutoDeleteTimerChangedMessage
    {
      AutoDeleteTime = 1
    }),
    (@"""migrate_to_chat_id"":1", new MigrateToChatMessage
    {
      ChatId = 1
    }),
    (@"""migrate_from_chat_id"":1", new MigrateFromChatMessage
    {
      ChatId = 1
    }),
    (@"""pinned_message"":{", new PinnedMessage
    {
      Pinned = new TextMessage()
    }),
    (@"""invoice"":{", new InvoiceMessage
    {
      Invoice = new Invoice()
    }),
    (@"""successful_payment"":{",
      new SuccessfulPaymentMessage
      {
        Payment = new SuccessfulPayment()
      }),
    (@"""connected_website"":""", new ConnectedWebsiteMessage
    {
      Website = Uri
    }),
    (@"""video_chat_scheduled"":{", new VideoChatScheduledMessage
    {
      Scheduled = new()
    }),
    (@"""video_chat_started"":{", new VideoChatStartedMessage
    {
      Started = new()
    }),
    (@"""video_chat_ended"":{", new VideoChatEndedMessage
    {
      Ended = new(0)
    }),
    (@"""video_chat_participants_invited"":{",
      new VideoChatParticipantsInvitedMessage
      {
        ParticipantsInvited = new(new List<User>())
      }),
    (@"""web_app_data"":{", new WebAppDataMessage
    {
      Data = new("", "")
    }),
    (@"""passport_data"":{", new PassportDataMessage
    {
      PassportData = new PassportData(
        new List<EncryptedElement>(), new EncryptedCredentials("", "", ""))
    }),
    (@"""proximity_alert_triggered"":{",
      new ProximityAlertTriggeredMessage
      {
        ProximityAlertTriggered = new ProximityAlertTriggered()
      }),
    (@"""file_id"":""", new PassportFile
    {
      Id = ""
    }),
    (@"""file_unique_id"":""", new PassportFile
    {
      UniqueId = ""
    }),
    (@"""file_size"":1", new PassportFile
    {
      Size = 1
    }),
    (@"""file_date"":0", new PassportFile
    {
      Date = DateTime.UnixEpoch
    }),
    (@"""field_name"":""", new DataFieldError
    {
      Name = ""
    }),
    (@"""data_hash"":""", new DataFieldError
    {
      Hash = ""
    }),
    (@"""element_hash"":""", new UnspecifiedError
    {
      Hash = ""
    }),
    (@"""file_hash"":""", new FrontSideError
    {
      Hash = ""
    }),
    (@"""file_hashes"":[", new FilesError
    {
      Hashes = new List<string>()
    }),
    (@"""update_id"":1", new MessageUpdate
    {
      Id = 1, Data = new TextMessage()
    }),
    (@"""message"":{", new MessageUpdate
    {
      Data = new TextMessage()
    }),
    (@"""edited_message"":{", new EditedMessageUpdate
    {
      Data = new TextMessage()
    }),
    (@"""channel_post"":{", new ChannelPostUpdate
    {
      Data = new TextMessage()
    }),
    (@"""edited_channel_post"":{", new EditedChannelPostUpdate
    {
      Data = new TextMessage()
    }),
    (@"""inline_query"":{", new InlineQueryUpdate
    {
      Data = new InlineQuery()
    }),
    (@"""chosen_inline_result"":{",
      new ChosenInlineResultUpdate
      {
        Data = new ChosenInlineResult()
      }),
    (@"""callback_query"":{", new CallbackQueryUpdate
    {
      Data = new MessageCallbackQuery()
    }),
    (@"""shipping_query"":{", new ShippingQueryUpdate
    {
      Data = new ShippingQuery()
    }),
    (@"""pre_checkout_query"":{", new PreCheckoutQueryUpdate
    {
      Data = new PreCheckoutQuery()
    }),
    (@"""poll"":{", new PollUpdate
    {
      Data = new RegularPoll()
    }),
    (@"""poll_answer"":{", new PollAnswerUpdate
    {
      Data = new PollAnswer()
    }),
    (@"""my_chat_member"":{", new MyChatMemberUpdate
    {
      Data = new ChatMemberUpdated()
    }),
    (@"""chat_member"":{", new ChatMemberUpdate
    {
      Data = new ChatMemberUpdated()
    }),
    (@"""chat_join_request"":{", new ChatJoinRequestUpdate
    {
      Data = new ChatJoinRequest()
    }),
    (@"""photos"":[", new UserProfilePhotos(0, new List<Photo[]>())),
    (@"""callback_query_id"":""", new AnswerCallbackQuery("", "")),
    (@"""inline_message_id"":""", new EditCaption("")),
    (@"""inline_message_id"":""", new EditLiveLocation("", 0, 0)),
    (@"""inline_message_id"":""",
      new EditMediaViaCache("", new CachedPhoto(""))),
    (@"""inline_message_id"":""", new EditMediaViaUrl("", new PhotoUrl(Uri))),
    (@"""inline_message_id"":""", new EditLiveLocation("", 0, 0)),
    (@"""inline_message_id"":""", new EditReplyMarkup("")),
    (@"""inline_message_id"":""", new EditText("", "")),
    (@"""file_id"":""", new GetFile("")),
    (@"""inline_message_id"":""", new StopLiveLocation("")),
    (@"""inline_query_id"":""",
      new AnswerInlineQuery("", new List<InlineResult>())),
    (@"""web_app_query_id"":""",
      new AnswerWebAppQuery("", new InlinePhoto("", Uri, Uri))),
    (@"""inline_message_id"":""", new GetInlineGameHighScores("", 1)),
    (@"""game_short_name"":""", new SendGame(1, "")),
    (@"""inline_message_id"":""", new SetInlineGameScore("", 1, 1)),
    (@"""disable_edit_message"":true", new SetGameScore(1, 1, 1, 1)
    {
      DisableEdit = true
    }),
    (@"""pre_checkout_query_id"":""", new AnswerPreCheckoutQuery("", "")),
    (@"""shipping_query_id"":""", new AnswerShippingQuery("", "")),
    (@"""photo_url"":""",
      new SendInvoice(1, "", "", "", "", "", new LabeledPrice[1])
      {
        Photo = Uri
      }),
    (@"""photo_url"":""",
      new CreateInvoiceLink("", "", "", "", "", new LabeledPrice[1])
      {
        Photo = Uri
      }),
    (@"""png_sticker"":""", new AddStickerToSetViaCache(1, "", "", "")),
    (@"""png_sticker"":""", new AddStickerToSetViaUrl(1, "", "", Uri)),
    (@"""png_sticker"":""attach://",
      new AddStickerToSetViaFile(1, "", "", File)),
    (@"""tgs_sticker"":""attach://",
      new AddAnimatedStickerToSetViaFile(1, "", "", File)),
    (@"""webm_sticker"":""attach://",
      new AddVideoStickerToSetViaFile(1, "", "", File)),
    (@"""png_sticker"":""", new CreateNewStickerSetViaCache(0, "", "", "", "")),
    (@"""png_sticker"":""", new CreateNewStickerSetViaUrl(0, "", "", "", Uri)),
    (@"""png_sticker"":""attach://",
      new CreateNewStickerSetViaFile(0, "", "", "", File)),
    (@"""tgs_sticker"":""attach://",
      new CreateNewAnimatedStickerSetViaFile(0, "", "", "", File)),
    (@"""webm_sticker"":""attach://",
      new CreateNewVideoStickerSetViaFile(0, "", "", "", File)),
    (@"""png_sticker"":""", new UploadStickerFile(1, Stream.Null)),
    (@"""message_thread_id"":1", new SendText(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendText("1", "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendCachedAnimation(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendCachedAnimation("1", "")
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendAnimationUrl(1, Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendAnimationUrl("1", Uri)
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendAnimationFile(1, File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendAnimationFile("1", File)
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendCachedAudio(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendCachedAudio("1", "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendAudioUrl(1, Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendAudioUrl("1", Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendAudioFile(1, File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendAudioFile("1", File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendCachedDocument(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendCachedDocument("1", "")
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendDocumentUrl(1, Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendDocumentUrl("1", Uri)
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendDocumentFile(1, File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1",
      new Requests.Usernames.SendDocumentFile("1", File)
      {
        ThreadId = 1
      }),
    (@"""message_thread_id"":1", new SendCachedPhoto(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendCachedPhoto("1", "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendPhotoUrl(1, Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendPhotoUrl("1", Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendPhotoFile(1, File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendPhotoFile("1", File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendCachedVideo(1, "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendCachedVideo("1", "")
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendVideoUrl(1, Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendVideoUrl("1", Uri)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new SendVideoFile(1, File)
    {
      ThreadId = 1
    }),
    (@"""message_thread_id"":1", new Requests.Usernames.SendVideoFile("1", File)
    {
      ThreadId = 1
    })
  };

  [Theory(DisplayName = "ContractResolver resolves properties correctly")]
  [MemberData(nameof(SerializationData))]
  public void ContractResolverResolvesPropertiesCorrectly(
    (string, object) tuple)
  {
    (string value, object data) = tuple;

    Assert.Contains(value, _serializer.Serialize(data), Ordinal);
  }
}

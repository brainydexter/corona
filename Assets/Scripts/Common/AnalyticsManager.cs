//using UnityEngine;

//public class AnalyticsManager : ISubscriberEvents
//{
//    private IEventAggregator m_eventAggregator;

//    public AnalyticsManager(IEventAggregator eventAggregator)
//    {
//        m_eventAggregator = eventAggregator;
//    }

////    private void HandleBoardInfoAnalyticsEvent(IEventType obj)
////    {
////#if !UNITY_EDITOR
////        var boardInfo = obj as BoardInfoAnalyticsEvent;
////        Analytics.CustomEvent("board_info", new Dictionary<string, object> {
////            { "board_id"     , boardInfo.BoardId},
////            { "falling_speed", boardInfo.FallingSpeed},
////        });
////#endif
////    }

////    /// <summary>
////    /// Handles the board start event and raises a level_start event
////    /// </summary>
////    /// <param name="obj"></param>
////    private void HandleBoardStartEvent(IEventType obj)
////    {
////#if !UNITY_EDITOR
////        var boardStartEvent = obj as BoardStartEvent;

////        AnalyticsEvent.LevelStart(boardStartEvent.BoardId);

////        if (boardStartEvent.BoardId <= 2)
////        {
////            AnalyticsEvent.TutorialStart(boardStartEvent.BoardId.ToString());
////        }
////#endif
////    }

////    internal void RaiseTutorialSkip(int boardId)
////    {
////        Debug.Assert(boardId <= 2, "[AnalyticsManager]: boardId not valid for skipping tutorial " + boardId);
////#if !UNITY_EDITOR
////        AnalyticsEvent.TutorialSkip(boardId.ToString());
////#endif
////    }

////    internal void RaiseTutorialComplete(int boardId)
////    {
////        Debug.Assert(boardId <= 2, "[AnalyticsManager]: boardId not valid for skipping tutorial " + boardId);
////#if !UNITY_EDITOR
////        AnalyticsEvent.TutorialComplete(boardId.ToString());
////#endif
////    }

//    public void RegisterEventHandlers()
//    {
//        //m_eventAggregator.Register<BoardCompleteEvent>(HandleBoardCompleteEvent);
//    }

//    public void UnRegisterEventHandlers()
//    {
//        if (m_eventAggregator != null)
//        {
//            //m_eventAggregator.UnRegister<BoardCompleteEvent>(HandleBoardCompleteEvent);
//        }
//    }
//}
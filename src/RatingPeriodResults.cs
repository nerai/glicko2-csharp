using System.Collections.Generic;

namespace Glicko2
{
	/// <summary>
	/// This class holds the results accumulated over a rating period.
	/// </summary>
	public class RatingPeriodResults
	{
		private readonly Dictionary<Rating, List<Result>> _results_ = new Dictionary<Rating, List<Result>> ();
		private readonly HashSet<Rating> _participants = new HashSet<Rating> ();

		/// <summary>
		/// Create an empty result set.
		/// </summary>
		public RatingPeriodResults ()
		{
		}

		/// <summary>
		/// Constructor that allows you to initialise the list of participants.
		/// </summary>
		/// <param name="participants"></param>
		public RatingPeriodResults (HashSet<Rating> participants)
		{
			_participants = participants;
		}

		private void AddResultForPlayer (Rating player, Result result)
		{
			List<Result> list;
			if (!_results_.TryGetValue (player, out list)) {
				list = new List<Result> ();
				_results_.Add (player, list);
			}
			list.Add (result);
		}

		/// <summary>
		/// Add a result to the set.
		/// </summary>
		/// <param name="winner"></param>
		/// <param name="loser"></param>
		public void AddResult (Rating winner, Rating loser)
		{
			var result = new Result (winner, loser);
			AddResultForPlayer (winner, result);
			AddResultForPlayer (loser, result);
		}

		/// <summary>
		/// Record a draw between two players and add to the set.
		/// </summary>
		/// <param name="player1"></param>
		/// <param name="player2"></param>
		public void AddDraw (Rating player1, Rating player2)
		{
			var result = new Result (player1, player2, true);
			AddResultForPlayer (player1, result);
			AddResultForPlayer (player2, result);
		}

		/// <summary>
		/// Get a list of the results for a given player.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public IList<Result> GetResults (Rating player)
		{
			List<Result> filteredResults;
			if (!_results_.TryGetValue (player, out filteredResults)) {
				filteredResults = new List<Result> ();
			}
			return filteredResults;
		}

		/// <summary>
		/// Get all the participants whose results are being tracked.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Rating> GetParticipants ()
		{
			// Run through the results and make sure all players have been pushed into the participants set.
			foreach (var player in _results_.Keys) {
				_participants.Add (player);
			}

			return _participants;
		}

		/// <summary>
		/// Add a participant to the rating period, e.g. so that their rating will
		/// still be calculated even if they don't actually compete.
		/// </summary>
		/// <param name="rating"></param>
		public void AddParticipant (Rating rating)
		{
			_participants.Add (rating);
		}

		/// <summary>
		/// Clear the result set.
		/// </summary>
		public void Clear ()
		{
			foreach (var result in _results_) {
				result.Value.Clear ();
			}
		}
	}
}

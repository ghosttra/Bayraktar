using System;
using System.Collections.Generic;
using System.Linq;
using BayraktarGame;

namespace Bayraktar
{
    public enum RatingSort
    {
        Date, Score
    }

    public enum RatingType
    {
        User, All
    }
    public class RatingControl
    {
        public RatingSort Sort { get; set; } = RatingSort.Date;
        public RatingType Type { get; set; } = RatingType.All;

        private static RatingControl _instance;
        public static RatingControl Instance => _instance ?? (_instance = new RatingControl());
        private RatingControl()
        {
            CurrentClient.Instance.Client.GetRating += _getRatingFromServer;
        }

        public Action<List<Statistic>> Result;
        public void GetRating()
        {
            CurrentClient.Instance.GetRating();
        }

        public void GetRating(RatingType type, RatingSort sort)
        {
            Sort = sort;
            Type = type;
            GetRating();
        }
        private void _getRatingFromServer(List<Statistic> rating)
        {
            Result?.Invoke(_handle(rating));
        }

        public Action NoData;
        private List<Statistic> _handle(List<Statistic> rating)
        {
            if (rating == null && rating.Count == 0)
            {
                NoData?.Invoke();
                return null;
            }
            var result = rating.AsEnumerable();
            switch (Type)
            {
                case RatingType.User:
                    result = result.Where(r => r.User?.Login.Equals(CurrentClient.Instance.Client.User.Login) == true);
                    break;
                case RatingType.All:
                    break;
            }

            switch (Sort)
            {
                case RatingSort.Date:
                    result = result.OrderBy(r => r.Date);
                    break;
                case RatingSort.Score:
                    result = result.OrderBy(r => r.Score);
                    break;
            }
            return result.ToList();
        }
    }
}
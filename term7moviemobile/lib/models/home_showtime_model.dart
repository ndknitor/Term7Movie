class HomeShowTimeModel {
  String? title;
  int? movieId;
  String? coverImgURL;
  int? theaterId;
  int? showTimeId;
  String? startTime;
  String? formattedStartTime;
  int? minutesRemain;
  double? distanceCalculated;
  double? minPrice;
  double? maxPrice;
  double? recommendPoint;
  String? theaterName;

  HomeShowTimeModel(
      {this.title,
        this.movieId,
        this.coverImgURL,
        this.theaterId,
        this.showTimeId,
        this.startTime,
        this.formattedStartTime,
        this.minutesRemain,
        this.distanceCalculated,
        this.minPrice,
        this.maxPrice,
        this.recommendPoint, this.theaterName});

  HomeShowTimeModel.fromJson(Map<String, dynamic> json) {
    title = json['title'];
    movieId = json['movieId'];
    coverImgURL = json['coverImgURL'];
    theaterId = json['theaterId'];
    showTimeId = json['showTimeId'];
    startTime = json['startTime'];
    formattedStartTime = json['formattedStartTime'];
    minutesRemain = json['minutesRemain'];
    distanceCalculated = json['distanceCalculated'];
    minPrice = json['minPrice'];
    maxPrice = json['maxPrice'];
    recommendPoint = json['recommendPoint'];
    theaterName = json['theaterName'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['title'] = this.title;
    data['movieId'] = this.movieId;
    data['coverImgURL'] = this.coverImgURL;
    data['theaterId'] = this.theaterId;
    data['showTimeId'] = this.showTimeId;
    data['startTime'] = this.startTime;
    data['formattedStartTime'] = this.formattedStartTime;
    data['minutesRemain'] = this.minutesRemain;
    data['distanceCalculated'] = this.distanceCalculated;
    data['minPrice'] = this.minPrice;
    data['maxPrice'] = this.maxPrice;
    data['recommendPoint'] = this.recommendPoint;
    data['theaterName'] = this.theaterName;
    return data;
  }
}
import 'package:term7moviemobile/models/category_model.dart';

class MovieModel {
  int? movieId;
  String? title;
  List<Categories>? categories;
  int? ageRestrict;
  int? duration;
  String? releaseDate;
  String? coverImgURL;
  String? posterImgURL;

  MovieModel(
      {this.movieId,
        this.title,
        this.categories,
        this.ageRestrict,
        this.duration,
        this.releaseDate,
        this.coverImgURL,
        this.posterImgURL});

  MovieModel.fromJson(Map<String, dynamic> json) {
    movieId = json['movieId'];
    title = json['title'];
    if (json['categories'] != null) {
      categories = <Categories>[];
      json['categories'].forEach((v) {
        categories!.add(new Categories.fromJson(v));
      });
    }
    ageRestrict = json['ageRestrict'];
    duration = json['duration'];
    releaseDate = json['releaseDate'];
    coverImgURL = json['coverImgURL'];
    posterImgURL = json['posterImgURL'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['movieId'] = this.movieId;
    data['title'] = this.title;
    if (this.categories != null) {
      data['categories'] = this.categories!.map((v) => v.toJson()).toList();
    }
    data['ageRestrict'] = this.ageRestrict;
    data['duration'] = this.duration;
    data['releaseDate'] = this.releaseDate;
    data['coverImgURL'] = this.coverImgURL;
    data['posterImgURL'] = this.posterImgURL;
    return data;
  }

}
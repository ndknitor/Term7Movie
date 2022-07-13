import 'package:term7moviemobile/models/category_model.dart';

class MovieModel {
  int? movieId;
  String? title;
  List<Categories>? categories;
  int? ageRestrict;
  int? duration;
  String? releaseDate;
  String? coverImgUrl;
  String? posterImgUrl;
  String? trailerUrl;
  String? description;
  List<String>? languageList;
  List<String>? actors;
  String? director;

  MovieModel(
      {this.movieId,
      this.title,
      this.categories,
      this.ageRestrict,
      this.duration,
      this.releaseDate,
      this.coverImgUrl,
      this.posterImgUrl,
      this.trailerUrl,
      this.description,
      this.languageList,
      this.actors,
      this.director});

  MovieModel.fromJson(Map<String, dynamic> json) {
    movieId = json['movieId'];
    title = json['title'];
    if (json['categories'] != null) {
      categories = <Categories>[];
      json['categories'].forEach((v) {
        categories!.add(new Categories.fromJson(v));
      });
    }
    ageRestrict = json['restrictedAge'];
    duration = json['duration'];
    releaseDate = json['releaseDate'];
    coverImgUrl = json['coverImgURL'] ?? json['coverImageUrl'];
    posterImgUrl = json['posterImgURL'] ?? json['posterImageUrl'];
    trailerUrl = json['trailerUrl'];
    description = json['description'];
    if (json['languageList'] != null) {
      languageList = [];
      json['languageList'].forEach((v) {
        languageList!.add(v);
      });
    }
    director = json['director'];
    if (json['beautifyActors'] != null) {
      actors = [];
      json['beautifyActors'].forEach((v) {
        actors!.add(v);
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['movieId'] = this.movieId;
    data['title'] = this.title;
    if (this.categories != null) {
      data['movieTypes'] = this.categories!.map((v) => v.toJson()).toList();
    }
    if (this.languageList != null) {
      data['languageList'] = this.languageList!.toList();
    }
    data['restrictedAge'] = this.ageRestrict;
    data['duration'] = this.duration;
    data['releaseDate'] = this.releaseDate;
    data['coverImgURL'] = this.coverImgUrl;
    data['posterImgURL'] = this.posterImgUrl;
    data['trailerUrl'] = this.trailerUrl;
    data['description'] = this.description;
    data['director'] = this.director;
    if (this.actors != null) {
      data['beautifyActors'] = this.actors!.toList();
    }
    return data;
  }
}

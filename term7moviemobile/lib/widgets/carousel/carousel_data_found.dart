import 'package:cached_network_image/cached_network_image.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter/material.dart';
import 'package:term7moviemobile/models/movie_model.dart';

class CarouselSliderDataFound extends StatefulWidget {
  final List<MovieModel> carouselList;
  const CarouselSliderDataFound({Key? key, required this.carouselList})
      : super(key: key);

  @override
  State<CarouselSliderDataFound> createState() =>
      _CarouselSliderDataFoundState();
}

class _CarouselSliderDataFoundState extends State<CarouselSliderDataFound> {
  int _current = 0;
  late List<Widget> imageSlider;

  @override
  void initState() {
    imageSlider = widget.carouselList
        .map((e) => Container(
      child: Container(
        margin: EdgeInsets.all(5.0),
        child: ClipRRect(
            borderRadius: BorderRadius.all(Radius.circular(14.0)),
            child: Stack(
              children: <Widget>[
                CachedNetworkImage(
                  imageUrl: e.coverImgURL ?? '',
                  errorWidget: (context, url, error) =>
                      Icon(Icons.error),
                  progressIndicatorBuilder: (context, url, downloadProgress) =>
                      Center(
                        child: CircularProgressIndicator(
                          value: downloadProgress.progress,
                        ),
                      ),
                  fit: BoxFit.cover,
                  width: 1000,
                  height: 1000,
                ),
                Positioned(
                  bottom: 0.0,
                  left: 0.0,
                  right: 0.0,
                  child: Container(
                    decoration: BoxDecoration(
                      gradient: LinearGradient(
                        colors: [
                          Color.fromARGB(200, 0, 0, 0),
                          Color.fromARGB(0, 0, 0, 0)
                        ],
                        begin: Alignment.bottomCenter,
                        end: Alignment.topCenter,
                      ),
                    ),
                    padding: EdgeInsets.symmetric(
                        vertical: 10.0, horizontal: 20.0),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          e.title ?? '',
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 20.0,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        // Text(
                        //   'Price: 80000',
                        //   style: TextStyle(
                        //     color: Colors.white,
                        //     fontSize: 20.0,
                        //     fontWeight: FontWeight.bold,
                        //   ),
                        // ),
                      ],
                    ),
                  ),
                ),
              ],
            )),
      ),
    ))
        .toList();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        children: [
          CarouselSlider(
              items: imageSlider,
              options: CarouselOptions(
                  autoPlay: true,
                  enlargeCenterPage: true,
                  aspectRatio: 16/9,
                  // viewportFraction: 1,
                  onPageChanged: (index, reason) {
                    setState(() {
                      _current = index;
                    });
                  })),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: widget.carouselList.map((e) {
              int index = widget.carouselList.indexOf(e);
              return Container(
                width: 8,
                height: 8,
                margin: EdgeInsets.symmetric(vertical: 10,horizontal: 2),
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: _current == index
                      ? Color.fromRGBO(0, 0, 0, 0.9)
                      : Color.fromRGBO(0, 0, 0, 0.4),
                ),
              );
            }).toList(),
          )
        ],
      ),
    );
  }
}

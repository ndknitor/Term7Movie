import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:term7moviemobile/models/home_showtime_model.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class MovieItem extends StatelessWidget {
  final HomeShowTimeModel data;
  const MovieItem({Key? key, required this.data}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(left: 10.0),
      child: GestureDetector(
        onTap: () {
          Get.toNamed("/booking/${data.showTimeId}");
          // Get.toNamed("/detail/${data.movieId}");
        },
        child: SingleChildScrollView(
          physics: const NeverScrollableScrollPhysics(),
          child: Container(
            width: 220,
            decoration: BoxDecoration(
              border: Border.all(color: MyTheme.borderColor),
              borderRadius: BorderRadius.circular(10),
            ),
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                mainAxisSize: MainAxisSize.max,
                children: [
                  ClipRRect(
                    borderRadius: BorderRadius.circular(10),
                    child: CachedNetworkImage(
                      imageUrl: data.coverImgURL == '' ? Constants.defaultImage : data.coverImgURL ?? Constants.defaultImage,
                      width: 200,
                      height: 150,
                      fit: BoxFit.cover,
                    ),
                  ),
                  const SizedBox(
                    height: 16,
                  ),
                  Container(
                    width: 200,
                    child: Text(
                      data.title ?? '',
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      softWrap: true,
                      style: TextStyle(
                          fontSize: 16,
                          color: MyTheme.textColor,
                          fontWeight: FontWeight.w600),
                    ),
                  ),
                  SizedBox(
                    height: 8,
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Icon(Icons.add_location_alt_rounded,
                          color: MyTheme.bottomBarColor, size: 16),
                      Text(
                        data.theaterName ?? '',
                        style: const TextStyle(
                            fontSize: 12, fontWeight: FontWeight.w500),
                      ),
                      SizedBox(
                        width: 16,
                      ),
                      Text(
                        (data.distanceCalculated!/1000).toStringAsFixed(2) +
                            "km",
                        style: const TextStyle(
                            fontSize: 12, fontWeight: FontWeight.w600),
                      ),
                    ],
                  ),
                  SizedBox(
                    height: 8,
                  ),
                  Row(
                    mainAxisSize: MainAxisSize.max,
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Row(
                        children: [
                          Icon(Icons.date_range,
                              color: MyTheme.bottomBarColor, size: 16),
                          SizedBox(
                            width: 4,
                          ),
                          Text(
                            data.formattedStartTime?.split(' ')[0] ?? '',
                            textAlign: TextAlign.center,
                            style: const TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w600,
                                color: MyTheme.grayColor),
                          ),
                        ],
                      ),
                      Container(
                        decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(4),
                            color: MyTheme.primaryColor),
                        padding:
                            EdgeInsets.symmetric(vertical: 4, horizontal: 8),
                        child: Text(
                          DateFormat.Hm().format(DateTime.parse(data.startTime!).add(Duration(hours: 7))),
                          textAlign: TextAlign.center,
                          style: const TextStyle(
                              fontSize: 12,
                              fontWeight: FontWeight.w600,
                              color: Colors.white),
                        ),
                      ),
                    ],
                  ),
                  SizedBox(
                    height: 6,
                  ),
                  Row(
                    children: [
                      const Icon(
                        Icons.attach_money,
                        size: 16,
                      ),
                      Text(
                        data.minPrice.toString() +
                            "VND - " +
                            data.maxPrice.toString() + "VND",
                        style: const TextStyle(
                            fontSize: 12, fontWeight: FontWeight.w600),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/showtime_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class CompanyList extends StatefulWidget {
  final String movieId;
  const CompanyList({Key? key, required this.movieId}) : super(key: key);

  @override
  State<CompanyList> createState() => _CompanyListState();
}

class _CompanyListState extends State<CompanyList> {
  @override

  void initState() {
    super.initState();
    ShowtimeController.instance.fetchCompanies();
  }

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Obx(() => LoadingOverlay(
              isLoading: ShowtimeController.instance.isLoading.value,
              color: MyTheme.backgroundColor,
              progressIndicator: const CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
              ),
              opacity: 1,
              child: ListView.builder(
                physics: const NeverScrollableScrollPhysics(),
                itemBuilder: (_, i) {
                  return Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      Expanded(
                        child: Container(
                          margin: EdgeInsets.fromLTRB(8, 0, 8, 12),
                          decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(10),
                              border: Border.all(
                                  color: MyTheme.borderColor, width: 1)),
                          child: Column(
                            children: [
                              GestureDetector(
                                onTap: () {
                                  if (ShowtimeController.instance.isSelected.value != i) {
                                    ShowtimeController.instance.isSelected.value = i;
                                  } else {
                                    ShowtimeController.instance.isSelected.value = -1;
                                  }
                                },
                                child: Container(
                                  padding: EdgeInsets.symmetric(
                                      vertical: 12, horizontal: 8),
                                  decoration: BoxDecoration(
                                      color: Colors.black.withOpacity(0.05),
                                      borderRadius: BorderRadius.circular(10)),
                                  child: Row(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                    children: [
                                      Row(
                                        children: [
                                          CircleAvatar(
                                            radius: 22,
                                            backgroundColor: Colors.transparent,
                                            backgroundImage: NetworkImage(
                                                ShowtimeController.instance
                                                        .companies[i].logoUrl ??
                                                    Constants.defaultImage),
                                          ),
                                          SizedBox(width: 6),
                                          Column(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              Text(
                                                ShowtimeController.instance.companies[i].name ??
                                                    '',
                                                style: TextStyle(
                                                    color: MyTheme.textColor,
                                                    fontWeight: FontWeight.w500,
                                                    fontSize: 16),
                                              ),
                                              SizedBox(height: 4),
                                              Text(
                                                ShowtimeController.instance.companies[i]
                                                        .theaters!.length
                                                        .toString() +
                                                    " theater",
                                                style: TextStyle(
                                                  color: MyTheme.textColor,
                                                  fontWeight: FontWeight.w400,
                                                  fontSize: 12,
                                                ),
                                              ),
                                            ],
                                          ),
                                        ],
                                      ),
                                      Icon(Icons.keyboard_arrow_down,
                                          color: MyTheme.textColor, size: 22)
                                    ],
                                  ),
                                ),
                              ),
                              Obx(() => ShowtimeController.instance.isSelected.value == i
                                  ? Container(
                                      width: double.infinity,
                                      padding: EdgeInsets.symmetric(
                                          vertical: 12, horizontal: 8),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: ShowtimeController.instance
                                            .companies[i].theaters!
                                            .map((e) => Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                                  children: [
                                                    GestureDetector(
                                                        onTap: () {
                                                          ShowtimeController.instance.theaterId.value = e.id!;
                                                          ShowtimeController.instance.movieId = widget.movieId;
                                                          ShowtimeController.instance.fetchShowtimes();
                                                        },
                                                        child:Padding(
                                                          padding:
                                                          const EdgeInsets.only(
                                                              top: 8.0,
                                                              bottom: 8.0),
                                                          child: Text(e.name ?? ''),
                                                        ),
                                                    ),
                                                    ShowtimeController.instance.theaterId.value == e.id! ?
                                                    Row(
                                                      children:
                                                      ShowtimeController.instance.showtimes.length == 0 ? [Container()] :
                                                      ShowtimeController.instance.showtimes.map((showtime) =>  GestureDetector(
                                                            onTap: () {
                                                              Get.toNamed("/booking/${showtime.id}");
                                                            },
                                                            child: Container(
                                                            margin: EdgeInsets.only(right: 8),
                                                            decoration: BoxDecoration(
                                                                borderRadius:
                                                                BorderRadius
                                                                    .circular(4),
                                                                color: MyTheme
                                                                    .primaryColor),
                                                            padding:
                                                            EdgeInsets.symmetric(
                                                                vertical: 8,
                                                                horizontal: 12),
                                                            child: Text(
                                                              DateFormat.Hm().format(DateTime.parse(showtime.startTime!).add(Duration(hours: 7))),
                                                              textAlign: TextAlign.center,
                                                              style: const TextStyle(
                                                                  fontSize: 12,
                                                                  fontWeight:
                                                                  FontWeight.w600,
                                                                  color:
                                                                  Colors.white),
                                                            ),
                                                        ),
                                                          )).toList(),
                                                    ) : Container(),
                                                  ],
                                                ))
                                            .toList(),
                                      ),
                                    )
                                  : Container())
                            ],
                          ),
                        ),
                      ),
                    ],
                  );
                },
                itemCount: ShowtimeController.instance.companies.length,
              ),
            )),
      ),
    );
  }
}

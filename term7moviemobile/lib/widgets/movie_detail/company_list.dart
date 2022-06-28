import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/showtime_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class CompanyList extends StatefulWidget {
  const CompanyList({Key? key}) : super(key: key);

  @override
  State<CompanyList> createState() => _CompanyListState();
}

class _CompanyListState extends State<CompanyList> {
  ShowtimeController controller = Get.put(ShowtimeController());

  @override
  void initState() {
    super.initState();
    controller.fetchCompanies();
  }

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Obx(() => LoadingOverlay(
              isLoading: controller.isLoading.value,
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
                                  if (controller.isSelected.value != i) {
                                    controller.isSelected.value = i;
                                  } else {
                                    controller.isSelected.value = -1;
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
                                                controller
                                                        .companies[i].logoUrl ??
                                                    Constants.defaultImage),
                                          ),
                                          SizedBox(width: 6),
                                          Column(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              Text(
                                                controller.companies[i].name ??
                                                    '',
                                                style: TextStyle(
                                                    color: MyTheme.textColor,
                                                    fontWeight: FontWeight.w500,
                                                    fontSize: 16),
                                              ),
                                              SizedBox(height: 4),
                                              Text(
                                                controller.companies[i]
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
                              Obx(() => controller.isSelected.value == i
                                  ? Container(
                                      width: double.infinity,
                                      padding: EdgeInsets.symmetric(
                                          vertical: 12, horizontal: 8),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: controller
                                            .companies[i].theaters!
                                            .map((e) => Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                                  children: [
                                                    GestureDetector(
                                                        onTap: () {
                                                          controller.theaterId.value = e.id!;
                                                          controller.fetchShowtimes(e.id!);
                                                        },
                                                        child:Padding(
                                                          padding:
                                                          const EdgeInsets.only(
                                                              top: 8.0,
                                                              bottom: 8.0),
                                                          child: Text(e.name ?? ''),
                                                        ),
                                                    ),
                                                    controller.theaterId.value == e.id! ?
                                                    Row(
                                                      children:
                                                          controller.showtimes.length == 0 ? [Container(child: Text('There is no showtime now', style: TextStyle(fontSize: 12, color: MyTheme.grayColor),))] :
                                                          controller.showtimes.map((e) =>  GestureDetector(
                                                            onTap: () {
                                                              Get.toNamed("/booking/${e.id}");
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
                                                              DateFormat.Hm().format(DateTime.parse(e.startTime!).add(Duration(hours: 7))),
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
                itemCount: controller.companies.length,
              ),
            )),
      ),
    );
  }
}

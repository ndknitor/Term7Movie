import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/showtime_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/date_picker.dart';

class ShowTimeScreen extends StatefulWidget {
  const ShowTimeScreen({Key? key}) : super(key: key);

  @override
  State<ShowTimeScreen> createState() => _ShowTimeScreenState();

}

class _ShowTimeScreenState extends State<ShowTimeScreen> {
  ShowtimeController controller = Get.put(ShowtimeController());

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        MyDatePicker(),
        SizedBox(
          height: 16,
        ),
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: ListView.builder(
              itemBuilder: (context, _) {
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
                            Container(
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
                                        backgroundColor: Colors.red,
                                      ),
                                      SizedBox(width: 6),
                                      Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "Beta Cinemas",
                                            style: TextStyle(
                                                color: MyTheme.textColor,
                                                fontWeight: FontWeight.w500,
                                                fontSize: 16),
                                          ),
                                          SizedBox(height: 4),
                                          Text(
                                            "1 theater",
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
                            Container(
                                padding: EdgeInsets.symmetric(
                                    vertical: 12, horizontal: 8),
                                child: Row(
                                  children: [Text("Beta Quang Trung")],
                                ))
                          ],
                        ),
                      ),
                    ),
                  ],
                );
              },
              itemCount: 5,
            ),
          ),
        )
      ],
    );
  }
}

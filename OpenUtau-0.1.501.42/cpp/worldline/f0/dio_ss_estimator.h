#ifndef WORLDLINE_F0_DIO_SS_ESTIMATOR_H_
#define WORLDLINE_F0_DIO_SS_ESTIMATOR_H_

#include <vector>

#include "worldline/f0/dio_estimator.h"

namespace worldline {

class DioSsEstimator : public DioEstimator {
 public:
  void Estimate(const std::vector<double>& samples, int fs, double frame_ms,
                std::vector<double>* f0,
                std::vector<double>* time_axis) override;
};

}  // namespace worldline

#endif  // WORLDLINE_F0_DIO_SS_ESTIMATOR_H_

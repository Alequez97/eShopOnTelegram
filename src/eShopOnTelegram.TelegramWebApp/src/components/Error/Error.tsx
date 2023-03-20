import classNames from "classnames";
import React from "react";
import css from "./Error.module.scss";

export default function Error() {
  return (
    <>
      <div className={css.browser}>
        <div className={css.controls}>
          <i className={classNames(css.browserButton, css.browserClose)}></i>
          <i className={classNames(css.browserButton, css.browserMinimize)}></i>
          <i className={classNames(css.browserButton, css.browserMaximize)}></i>
        </div>

        <div className={css.eye}></div>
        <div className={css.eye}></div>
        <div className={css.mouth}>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
          <div className={css.lips}></div>
        </div>
      </div>
      <div>
        Unfortunately, we're unable to fulfill your request. Rest assured we
        have been notified and are looking into the issue. Please refresh your
        browser. If the error continues, please contact our{" "}
        <a href="#">support team</a>.
      </div>
    </>
  );
}

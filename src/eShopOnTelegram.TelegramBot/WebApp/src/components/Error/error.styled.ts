import styled, { css, keyframes } from "styled-components";

const browserBobbleAnimation = keyframes`
  0%, 100% {
    margin-top: 40px;
    margin-bottom: 48px;
    box-shadow: 0 40px 80px rgba(0, 0, 0, 0.24);
  }
  50% {
    margin-top: 54px;
    margin-bottom: 34px;
    box-shadow: 0 24px 64px rgba(0, 0, 0, 0.4);
  }
`;

export const StyledBrowser = styled.div`
  width: 300px;
  min-width: 200px;
  min-height: 264px;
  background: var(--tg-theme-secondary-bg-color);
  box-shadow: 0 40px 80px 0 rgba(0, 0, 0, 0.25);
  border-radius: px;
  animation: bobble 1.8s ease-in-out infinite;
  position: relative;
  left: 50%;
  margin-left: -150px;
  margin-top: 50px;
  animation: ${browserBobbleAnimation} 2s infinite;
`;

export const StyledControls = styled.div`
  width: 100%;
  height: 32px;
  background: #e8ecef;
  border-radius: 3px 3px 0 0;
  box-sizing: border-box;
  padding: 10px 12px;
`;

interface StyledBrowserButtonProps {
  type: "close" | "minimize" | "maximize";
}

const buttonStyles: Record<StyledBrowserButtonProps["type"], string> = {
  close: "rgb(222, 20, 20)",
  minimize: "rgb(221, 221, 8)",
  maximize: "rgb(22, 206, 22)",
};

export const StyledBrowserButton = styled.i<StyledBrowserButtonProps>`
  height: 12px;
  width: 12px;
  border-radius: 100%;
  display: block;
  float: left;
  margin-right: 8px;
  background-color: ${(props) => buttonStyles[props.type]};
`;

export const StyledEye = styled.div`
  position: absolute;
  left: 34%;
  top: 80px;
  width: 32px;
  height: 32px;
  opacity: 1;

  & + & {
    right: 34%;
    left: auto;
  }

  &:before,
  &:after {
    position: absolute;
    left: 15px;
    content: " ";
    height: 40px;
    width: 3px;
    border-radius: 2px;
    background-color: #ff5e5b;
  }
  &:before {
    transform: rotate(45deg);
  }
  &:after {
    transform: rotate(-45deg);
  }
`;

export const StyledMouth = styled.div`
  position: absolute;
  width: 125px;
  top: 178px;
  left: 50%;
  margin-left: -62.5px;
  height: 40px;
`;

const lipsMixin = (num: number) => css`
  ${Array.from(
    { length: num },
    (_, i) => `
    &:nth-child(n + ${i + 1}) {
      margin-left: ${i * 31}px;
    }
  `
  )}

  &:nth-child(odd) {
    transform: rotate(54deg);
  }
`;

export const StyledLips = styled.div`
  position: absolute;
  left: 15px;
  content: " ";
  height: 40px;
  width: 3px;
  border-radius: 2px;
  background-color: #ff5e5b;
  transform: rotate(-54deg);
  ${lipsMixin(4)};
`;

export const StyledErrorMessageContainer = styled.div`
  text-align: center;
  margin: 20px;

  p {
    font-family: Arial, sans-serif;
    font-size: 16px;
    line-height: 1.5;
    text-justify: auto;

    a {
      color: #007bff;
      text-decoration: none;
    }
  }
`;
